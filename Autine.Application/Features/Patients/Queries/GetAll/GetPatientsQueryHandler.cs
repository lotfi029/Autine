using Autine.Application.Contracts.Patients;

namespace Autine.Application.Features.Patients.Queries.GetAll;
public class GetPatientsQueryHandler(IUserService userService) : IQueryHandler<GetPatientsQuery, ICollection<PatientResponse>>
{
    public async Task<Result<ICollection<PatientResponse>>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
    {
        var respons = await userService
            .GetPatientsAsync(request.UserId, request.IsFollowing, cancellationToken);

        return respons.ToList();
    }
}
