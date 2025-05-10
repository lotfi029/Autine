using Autine.Application.IServices;
using Autine.Application.IServices.AIApi;

namespace Autine.Application.Features.Patients.Commads.Remove;
public class RemovePatientCommandHandler(
    IUnitOfWork unitOfWork,
    IUserService userService,
    IAIAuthService aIAuthService) : ICommandHandler<RemovePatientCommand>
{
    public async Task<Result> Handle(RemovePatientCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Patients.GetAsync(e => e.PatientId == request.Id && e.CreatedBy == request.UserId && e.IsSupervised, ct: cancellationToken) is not { } patient)
            return PatientErrors.PatientsNotFound;

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var deleteResult = await userService.DeleteUserAsync(patient.PatientId, cancellationToken, transaction);
            if (deleteResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return deleteResult;
            }

            var aiResult = await aIAuthService.RemovePatientAsync(request.UserId, request.Id, cancellationToken);
            if (aiResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return aiResult;
            }

            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success();
        }
        catch
        {
            // TODO: log error
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.BadRequest("Error", "An error occurred while removing the patient.");
        }
    }
}
