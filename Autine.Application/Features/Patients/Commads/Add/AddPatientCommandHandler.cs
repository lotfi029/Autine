namespace Autine.Application.Features.Patients.Commads.Add;
public class AddPatientCommandHandler(
    IUnitOfWork unitOfWork, 
    IAuthService authService, 
    IAIAuthService aIAuthService) : ICommandHandler<AddPatientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddPatientCommand request, CancellationToken ct)
    {

        var transaction = await unitOfWork.BeginTransactionAsync(ct);
        Guid patiendId = Guid.Empty;
        bool aIPatientAdded = false;
        string userPatientId = string.Empty;
        try
        {
            var authResult = await authService.RegisterPatient(request.Request, ct);

            if (authResult.IsFailure)
                return authResult.Error;

            userPatientId = authResult.Value;

            var aIResult = await aIAuthService.AddPatientAsync(
                request.UserId, new(
                    request.Request.Email,
                    userPatientId,
                    request.Request.Password,
                    request.Request.FirstName,
                    request.Request.LastName,
                    request.Request.DateOfBirth,
                    request.Request.Gender
                    ), ct);

            if (aIResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, ct);
                return aIResult.Error;
            }
            aIPatientAdded = true;

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
                    MemberId = request.UserId
                }, ct);

            patiendId = patient.Id;

        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(transaction, ct);
            
            if (aIPatientAdded)
                await aIAuthService.RemovePatientAsync(request.UserId, userPatientId, ct);
            // TODO: log error
            return Error.BadRequest("Error", "Error while adding patient");
        }

        return Result.Success(patiendId);
    }
}
