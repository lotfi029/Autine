namespace Autine.Application.Features.Patients.Commads.Add;
public class AddPatientCommandHandler(
    IUnitOfWork unitOfWork, 
    IAuthService authService, 
    IAIAuthService aIAuthService) : ICommandHandler<AddPatientCommand, string>
{
    public async Task<Result<string>> Handle(AddPatientCommand request, CancellationToken ct)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var authResult = await authService.RegisterPatient(request.Request, ct);

            if (authResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, ct);
                return authResult.Error;
            }
            string userPatientId = authResult.Value;

            var aIResult = await aIAuthService.AddPatientAsync(
                request.UserId, new(
                    request.Request.Email,
                    userPatientId,
                    Consts.FixedPassword,
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


            await unitOfWork.CommitTransactionAsync(transaction, ct);
            return Result.Success(authResult.Value);
        }
        catch
        {
            // TODO: log error
            await unitOfWork.RollbackTransactionAsync(transaction, ct);
            return Error.BadRequest("Error", "Error while adding patient");
        }
    }
}
