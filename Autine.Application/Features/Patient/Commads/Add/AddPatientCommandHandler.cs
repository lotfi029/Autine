namespace Autine.Application.Features.Patient.Commads.Add;
public class AddPatientCommandHandler(
    IUnitOfWork unitOfWork, 
    IAuthService authService, 
    IAIAuthService aIAuthService) : ICommandHandler<AddPatientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddPatientCommand request, CancellationToken ct)
    {
        var authResult = await authService.RegisterPatient(request.Request, ct);

        if (authResult.IsFailure)
            return authResult.Error;


        var aIResult = await aIAuthService.AddPatientAsync(
            request.UserId, new(
                request.Request.Email,
                authResult.Value,
                request.Request.Password,
                request.Request.FirstName,
                request.Request.LastName,
                request.Request.DateOfBirth,
                request.Request.Gender
                ), ct);

        if (aIResult.IsFailure)
            return aIResult.Error;

        var patientId = await unitOfWork.Patients.AddAsync(
            new()
            {
                IsSupervised = true,
                PatientId = authResult.Value,
                ThreadTitle = string.Empty
            }, ct);

        return Result.Success(patientId);
    }
}
