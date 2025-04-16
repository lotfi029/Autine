using Autine.Application.Contracts.Thread;

namespace Autine.Application.Features.ThreadMember.Queries.Get;
public class GetThreadMemberQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetThreadMemberQuery, ThreadResponse>
{
    public async Task<Result<ThreadResponse>> Handle(GetThreadMemberQuery request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Patients.FindByIdAsync(cancellationToken, [request.ThreadId]) is not { } thread)
            return PatientErrors.PatientsNotFound;

        if (thread.CreatedBy != request.UserId)
            return PatientErrors.PatientsNotFound;

        var members = await unitOfWork.ThreadMembers.GetAllAsync(e => e.PatientId == request.ThreadId, ct: cancellationToken);

        var threadMemberResponse = members.Select(e => new ThreadMemberResponse(e.Id, e.UserId, e.CreatedAt));

        var threadResponse = new ThreadResponse(
            thread.Id,
            thread.ThreadTitle,
            thread.CreatedBy,
            thread.PatientId,
            thread.CreatedAt,
            [.. threadMemberResponse]
        );

        return threadResponse;
    }
}
