namespace Autine.Application.Features.Patients.Commads.Update;
public class UpdatePatientCommandHandler(
    IAccountService accountService,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdatePatientCommand>
{
    public async Task<Result> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Patients.FindByIdAsync(cancellationToken, [request.PatientId]) is not { } patient)
            return PatientErrors.PatientsNotFound;

        if (patient.CreatedBy != request.UserId)
            return PatientErrors.PatientsNotFound;

        var userUpdateResult = await accountService.UpdateProfileAsync(patient.PatientId, request.UpdateRequest, cancellationToken);

        if (userUpdateResult.IsFailure)
            return userUpdateResult;
        // Update user info
        return Result.Success();
    }
}
