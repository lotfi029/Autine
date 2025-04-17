using Autine.Application.Contracts.Thread;

namespace Autine.Application.Features.Thread.Queries.Get;
public class GetThreadQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetThreadQuery, ThreadResponse>
{
    public async Task<Result<ThreadResponse>> Handle(GetThreadQuery request, CancellationToken cancellationToken)
    {
        var threadMember = await unitOfWork.ThreadMembers
            .GetAllAsync(e => e.PatientId == request.Id, ct: cancellationToken);

        if (threadMember is null || threadMember.Any())
            return PatientErrors.PatientsNotFound;

        if (!threadMember.Select(e => e.UserId).Contains(request.UserId))
            return PatientErrors.PatientsNotFound;


        if(await unitOfWork.Patients.GetAsync(e => e.Id == request.Id, ct: cancellationToken) is not { } thread)
            return PatientErrors.PatientsNotFound;


        var response = new ThreadResponse
        (
            Id: thread.Id,
            Title: thread.ThreadTitle,
            SupervisorId: thread.CreatedBy,
            PatientId: thread.PatientId,
            CraetedAt: thread.CreatedAt,
            ThreadMembers: [.. threadMember
                .Select(e => new ThreadMemberResponse
                (
                    Id: e.Id,
                    UserId: e.UserId,
                    e.CreatedAt
                ))]
        );

        return Result.Success(response);
    }
}
