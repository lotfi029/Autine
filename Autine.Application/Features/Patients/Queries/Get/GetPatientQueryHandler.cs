using Autine.Application.Contracts.Patients;

namespace Autine.Application.Features.Patients.Queries.Get;
public record GetPatientQueryHandler(IUserService userService) : IQueryHandler<GetPatientQuery, PatientResponse>
{
    public async Task<Result<PatientResponse>> Handle(GetPatientQuery request, CancellationToken cancellationToken)
    {
        if (await userService.GetPatientByIdAsync(request.UserId, request.Id, ct: cancellationToken) is not { } patient)
            return PatientErrors.PatientsNotFound;

        return patient;
    }
}
