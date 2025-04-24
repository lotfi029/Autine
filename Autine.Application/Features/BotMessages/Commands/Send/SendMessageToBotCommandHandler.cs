using Autine.Application.Contracts.UserBots;

namespace Autine.Application.Features.BotMessages.Commands.Send;
public class SendMessageToBotCommandHandler(
    IUnitOfWork unitOfWork,
    IAIModelService aIModelService
    ) : ICommandHandler<SendMessageToBotCommand, MessageResponse>
{
    public async Task<Result<MessageResponse>> Handle(SendMessageToBotCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.BotPatients.FindByIdAsync(cancellationToken,[request.BotPatientId]) is not { } botPatient)
            return BotErrors.BotNotFound;

        if (await unitOfWork.Bots.FindByIdAsync(cancellationToken, [botPatient.BotId]) is not { } bot)
            return BotErrors.BotNotFound; 

        var modelMessageResponse = await aIModelService.SendMessageToModelAsync(
            request.UserId,
            bot.Name,
            request.Content,
            cancellationToken
            );

        if (modelMessageResponse.IsFailure)
            return modelMessageResponse.Error;

        var messages = new List<Message>()
        {
            new()
            {
                Content = request.Content,
                SenderId = request.UserId,
                Status = MessageStatus.Read
            },
            new()
            {
                Content = modelMessageResponse.Value.model_msg,
                SenderId = null,
                Status = MessageStatus.Read
            }
        };
        
        var botMessage = new List<BotMessage>()
        {
            new()
            {
                BotPatientId = botPatient.Id,
                MessageId = messages[0].Id
            },
            new()
            {
                BotPatientId = botPatient.Id,
                MessageId = messages[1].Id
            }
        };

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Messages.AddRangeAsync(messages, cancellationToken);
            await unitOfWork.BotMessages.AddRangeAsync(botMessage, cancellationToken);
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
        }

        var response = new MessageResponse(
            botMessage[1].Id,
            modelMessageResponse.Value.model_msg, 
            DateTime.UtcNow, 
            MessageStatus.Read, 
            true);

        return response;
    }
}
  