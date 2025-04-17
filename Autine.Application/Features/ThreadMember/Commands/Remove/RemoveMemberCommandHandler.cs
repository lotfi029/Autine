namespace Autine.Application.Features.ThreadMember.Commands.Remove;
public class RemoveMemberCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<RemoveMemberCommand>
{
    public async Task<Result> Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.ThreadMembers.FindByIdAsync(cancellationToken, [request.ThreadMemberId]) is not { } threadMember)
            return ThreadMemberErrors.ThreadMemberNotFound;

        if (threadMember.CreatedBy != request.UserId) 
            return ThreadMemberErrors.InvalidThreadMember;

        unitOfWork.ThreadMembers.Delete(threadMember);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return Result.Success();
    }
}
