using Autine.Application.Contracts.Threads;

namespace Autine.Application.Features.ThreadMember.Commands.Messages;
public record SendMessageCommand(string UserId, ThreadMessageRequest ThreadMessage) : ICommand<Guid>;

public class SendMessageCommandHandler(
    IUnitOfWork unitOfWork) : ICommandHandler<SendMessageCommand, Guid>
{
    public async Task<Result<Guid>> Handle(SendMessageCommand request, CancellationToken ct)
    {
        if (!await unitOfWork.Patients.CheckExistAsync(e => e.Id == request.ThreadMessage.ThreadId, ct))
                return ThreadErrors.ThreadNotFound;

        if (await unitOfWork.ThreadMembers.GetAsync(e => e.MemberId == request.UserId && e.ThreadId == request.ThreadMessage.ThreadId, ct: ct) is not { } threadMember)
            return ThreadMemberErrors.ThreadMemberNotFound;


        var message = new Message
        {
            ThreadMemberId = threadMember.Id,
            Content = request.ThreadMessage.Content,
            SenderId = request.UserId,
            Status = MessageStatus.Read,
            ReadAt = DateTime.UtcNow
        };
        await unitOfWork.Messages.AddAsync(message, ct);
        await unitOfWork.CommitChangesAsync(ct);

        return Result.Success(message.Id);
    }
}