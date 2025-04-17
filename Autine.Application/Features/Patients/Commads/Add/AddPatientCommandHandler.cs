namespace Autine.Application.Features.Patients.Commads.Add;
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

        var patient = new Patient()
        {

            IsSupervised = true,
            PatientId = authResult.Value,
            ThreadTitle = $"{request.Request.FirstName} {request.Request.LastName}"
        };

        await unitOfWork.Patients.AddAsync(patient, ct);

        await unitOfWork.ThreadMembers.AddAsync(
            new()
            {
                PatientId = patient.Id,
                UserId = request.UserId
            }, ct);

        return Result.Success(patient.Id);
    }
}
