using Autine.Application.Contracts.Patient;

namespace Autine.Application.Features.Patient.Queries.GetAll;
public class GetPatientsQueryHandler(IUserService userService) : IQueryHandler<GetPatientsQuery, ICollection<PatientResponse>>
{
    public async Task<Result<ICollection<PatientResponse>>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
    {
        var respons = await userService.GetPatientsAsync(request.userId, cancellationToken);

        if (!respons.Any())
            return UserErrors.UserNotFound;

        return respons.ToList();
    }
}
