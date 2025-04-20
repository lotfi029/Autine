namespace Autine.Application.Features.Patients.Commads.Remove;
public class RemovePatientCommandHandler(
    IUnitOfWork unitOfWork,
    IUserService userService,
    IAIAuthService aIAuthService) : ICommandHandler<RemovePatientCommand>
{
    public async Task<Result> Handle(RemovePatientCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Patients.FindByIdAsync(cancellationToken, [request.Id]) is not { } patient)
            return PatientErrors.PatientsNotFound;

        if (patient.CreatedBy != request.UserId)
            return PatientErrors.PatientsNotFound;

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            unitOfWork.Patients.Delete(patient);
            await unitOfWork.CommitChangesAsync(cancellationToken);

            var result = await userService.DeleteUserAsync(patient.PatientId, cancellationToken);
        
            if (result.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return result;
            }
            var aiResult = await aIAuthService.RemovePatientAsync(request.UserId, patient.PatientId, cancellationToken);

            if (aiResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return aiResult;
            }
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.BadRequest("Error", "An error occurred while removing the patient.");
        }

        return Result.Success();
    }
}
