using Autine.Application.Contracts.Threads;

namespace Autine.Application.Features.ThreadMember.Queries.GetAll;
public class GetThreadMembersQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetThreadMembersQuery, IEnumerable<ThreadMemberResponse>>
{
    public async Task<Result<IEnumerable<ThreadMemberResponse>>> Handle(GetThreadMembersQuery request, CancellationToken cancellationToken)
    {
        
        if (await unitOfWork.Patients.GetAsync(e => e.PatientId == request.PatientId, ct: cancellationToken) is not { } thread)
            return PatientErrors.PatientsNotFound;

        var threadMember = await unitOfWork.ThreadMembers
            .GetAllAsync(e => e.ThreadId == thread.Id, ct: cancellationToken);

        var response = threadMember
            .Select(e => new ThreadMemberResponse
            (
                Id: e.Id,
                UserId: e.MemberId,
                e.CreatedAt
            ));

        return Result.Success(response);
    }
}
