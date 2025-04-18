using Autine.Application.Contracts.Threads;

namespace Autine.Application.Features.ThreadMember.Queries.GetAll;
public class GetThreadMembersQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetThreadMembersQuery, IEnumerable<ThreadMemberResponse>>
{
    public async Task<Result<IEnumerable<ThreadMemberResponse>>> Handle(GetThreadMembersQuery request, CancellationToken cancellationToken)
    {
        var threadMember = await unitOfWork.ThreadMembers
            .GetAllAsync(e => e.PatientId == request.PatientId, ct: cancellationToken);

        var response = threadMember
            .Select(e => new ThreadMemberResponse
            (
                Id: e.Id,
                UserId: e.UserId,
                e.CreatedAt
            ));

        return Result.Success(response);
    }
}
