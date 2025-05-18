
namespace Autine.Application.Features.Messages.Commands.DeleteChat;
public record DeleteChatCommand(string UserId, Guid ChatId) : ICommand;

public class DeleteChatCommandHandler(
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteChatCommand>
{
    public async Task<Result> Handle(DeleteChatCommand request, CancellationToken ct)
    {
        if (await unitOfWork.Chats.FindByIdAsync(ct, [request.ChatId]) is not { } chat)
            return ChatErrors.ChatNotFound;

        if (chat.UserId != request.UserId)
            return ChatErrors.ChatNotBelongToUser;

        var beginTransaction = await unitOfWork.BeginTransactionAsync(ct);

        try
        {
            await unitOfWork.Messages.ExcuteDeleteAsync(e => e.ChatId == request.ChatId, ct);

            await unitOfWork.Chats.ExcuteDeleteAsync(e => e.Id == request.ChatId, ct);

            await unitOfWork.CommitTransactionAsync(beginTransaction, ct);
            return Result.Success();
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(beginTransaction, ct);
            return Error.BadRequest("Error.DeleteChat", "Error occure while delete the chat");
        }
    }
}