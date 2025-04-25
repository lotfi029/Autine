using Autine.Application.Contracts.UserBots;

namespace Autine.Application.Features.UserBots.Commands.Send;
public class SendMessageToBotCommandHandler(
    IUnitOfWork unitOfWork,
    IAIModelService aIModelService
    ) : ICommandHandler<SendMessageToBotCommand, MessageResponse>
{
    public async Task<Result<MessageResponse>> Handle(SendMessageToBotCommand request, CancellationToken cancellationToken)
    {
        var botPatient = await unitOfWork.BotPatients
            .GetAsync(e => 
            e.BotId == request.BotId && 
            e.UserId == request.UserId,
            includes: "Bot",
            ct: cancellationToken);


        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            string botName = string.Empty;

            if (botPatient == null)
            {
                var bot = await unitOfWork.Bots
                    .GetAsync(e => e.Id == request.BotId, ct: cancellationToken);

                if (!bot.IsPublic)
                    return BotErrors.InvalidBot;

                await unitOfWork.BotPatients.AddAsync(new()
                {
                    BotId = bot.Id,
                    UserId = request.UserId
                }, ct: cancellationToken);

                botName = bot.Name;
            }
            else
                botName = botPatient.Bot.Name;

            var userMessage = new Message()
            {
                SenderId = request.UserId,
                Content = request.Content,
                CreatedDate = DateTime.UtcNow,
                DeliveredAt = DateTime.UtcNow,
                ReadAt = DateTime.UtcNow,
                Status = MessageStatus.Read
            };

            var userBotMessage = new BotMessage()
            {
                MessageId = userMessage.Id,
                BotPatientId = request.BotId
            };

            var botResponse = await aIModelService.SendMessageToModelAsync(
                userId: request.UserId,
                modelName: botName,
                message: request.Content,
                ct: cancellationToken
                );

            if (botResponse.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return BotMessageError.FailedToSendMessage;
            }

            var botMessage = new Message()
            {
                Content = botResponse.Value.model_msg,
                CreatedDate = DateTime.UtcNow,
                DeliveredAt = DateTime.UtcNow,
                ReadAt = DateTime.UtcNow,
                Status = MessageStatus.Read
            };

            var botBotMessage = new BotMessage()
            {
                MessageId = botMessage.Id,
                BotPatientId = request.BotId
            };


            await unitOfWork.Messages.AddRangeAsync([userMessage, botMessage], cancellationToken);
            await unitOfWork.BotMessages.AddRangeAsync([userBotMessage, botBotMessage], cancellationToken);
            await unitOfWork.CommitChangesAsync(cancellationToken);

            var response = new MessageResponse(
                botMessage.Id, 
                botMessage.Content, 
                botMessage.CreatedDate, 
                botMessage.Status, 
                true);

            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success(response);
        }
        catch
        {
            // TODO: log error
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.BadRequest("SendMessage.Error", "Error occure while sendimg message");
        }
    }
}
