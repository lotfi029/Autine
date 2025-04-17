using Autine.Application.Contracts.Thread;

namespace Autine.Application.Features.ThreadMember.Queries.Get;
public class GetThreadMemberQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetThreadMemberQuery, ThreadMemberResponse>
{
    public async Task<Result<ThreadMemberResponse>> Handle(GetThreadMemberQuery request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.ThreadMembers.GetAsync(e => e.Id == request.Id, ct: cancellationToken) is not { } thread)
            return PatientErrors.PatientsNotFound;

        var response = new ThreadMemberResponse(
            thread.Id,
            thread.UserId,
            thread.CreatedAt
            );

        return Result.Success(response);
    }
}
