namespace Autine.Application.Features.Bots.Commands.UnAssign;
public class DeleteAssignCommandHandler(
    IUnitOfWork unitOfWork,
    IAIModelService aIModelService) : ICommandHandler<DeleteAssignCommand>
{
    public async Task<Result> Handle(DeleteAssignCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.BotPatients.GetAsync(e => !e.IsDisabled && e.Id == request.BotPatientId, ct: cancellationToken) is not { } botPatient)
            return BotErrors.BotNotFound;

        if (await unitOfWork.Bots.GetAsync(e => !e.IsDisabled && e.Id == botPatient.BotId && e.CreatedBy == request.UserId, ct: cancellationToken) is not { } bot)
            return BotErrors.BotNotFound;

        if (await unitOfWork.Patients.GetAsync(e => !e.IsDisabled && e.Id == botPatient.PatientId && e.CreatedBy == request.UserId, ct: cancellationToken) is not { } patient)
            return PatientErrors.PatientsNotFound;



        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.BotPatients.DeleteBotPatientAsync(bot, cancellationToken);

            var aiResult = await aIModelService.UnAssignModelAsync(
                request.UserId,
                patient.PatientId,
                bot.Name,
                cancellationToken
                );

            if (aiResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return aiResult;
            }

            await unitOfWork.CommitTransactionAsync(transaction,cancellationToken);
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
