using Autine.Application.Contracts.UserBots;

namespace Autine.Application.Features.Messages.Commands;

public class SendDMCommandHandler(
    IUnitOfWork unitOfWork,
    IRoleService roleService) : ICommandHandler<SendDMCommand, MessageResponse>
{
    public async Task<Result<MessageResponse>> Handle(SendDMCommand request, CancellationToken ct)
    {
        if (!await roleService.UserIsSupervisorAsync(request.RecieverId))
            return UserErrors.UserNotFound;

        var n = string.CompareOrdinal(request.UserId, request.RecieverId) > 0;
        var userId = n ? request.UserId : request.RecieverId;
        var memberId = !n ? request.UserId : request.RecieverId;

        var chat = await unitOfWork.Chats.GetAsync(e => e.CreatedBy == userId && e.UserId == memberId, ct: ct);

        var transaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var chatId = Guid.Empty;
            if (chat == null)
            {
                chat = new Chat
                {
                    CreatedBy = userId,
                    UserId = userId
                };
                await unitOfWork.Chats.AddAsync(chat, ct);
                chatId = chat.Id;
            }
            else
            {
                chatId = chat.Id;
            }

            var userMessage = new Message
            {
                Content = request.Content,
                ChatId = chatId,
                SenderId = request.UserId
            };

            await unitOfWork.Messages.AddAsync(userMessage, ct);

            var response = new MessageResponse(
                userMessage.Id,
                userMessage.Content,
                userMessage.CreatedDate,
                userMessage.Status,
                true
                );

            await unitOfWork.CommitTransactionAsync(transaction, ct);
            return response;
        }
        catch
        {
            // TODO: log error
            await unitOfWork.RollbackTransactionAsync(transaction, ct);
            return Error.BadRequest("Error.SendMessage", "error while sending message.");
        }
    }
}
