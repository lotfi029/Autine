using Autine.Application.Contracts.Thread;

namespace Autine.Application.Features.ThreadMember.Queries.GetAll;
public class GetThreadMembersQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetThreadMembersQuery, IEnumerable<ThreadResponse>>
{
    public async Task<Result<IEnumerable<ThreadResponse>>> Handle(GetThreadMembersQuery request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Patients.GetAllAsync(e => e.CreatedBy == request.UserId, ct: cancellationToken) is not { } threads)
            return PatientErrors.PatientsNotFound;

        var members = await unitOfWork.ThreadMembers.GetAllAsync(e => request.UserId == e.CreatedBy, ct: cancellationToken);

        var response = threads.Join(members, t => t.Id, m => m.PatientId, (t, m) => new
        {
            t.Id,
            t.ThreadTitle,
            t.CreatedBy,
            t.PatientId,
            t.CreatedAt,
            ThreadMemeberId = m.Id,
            ThreadMemeberUserId = m.UserId,
            ThreadMemeberCreatedAt = m.CreatedAt
        })
        .GroupBy(g => new
        {
            g.Id,
            g.ThreadTitle,
            g.CreatedBy,
            g.PatientId,
            g.CreatedAt,
        })
        .Select(e => new ThreadResponse
        (
            e.Key.Id,
            e.Key.ThreadTitle,
            e.Key.CreatedBy,
            e.Key.PatientId,
            e.Key.CreatedAt,
            [.. e.Select(m => new ThreadMemberResponse
            (
                m.ThreadMemeberId,
                m.ThreadMemeberUserId,
                m.ThreadMemeberCreatedAt
            ))]
        ));

        return Result.Success(response);
    }
}
