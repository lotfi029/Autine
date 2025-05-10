using Autine.Application.IServices.AIApi;

namespace Autine.Application.Features.UserBots.Commands.Remove;

public class DeleteChatCommandHandler(
    IUnitOfWork unitOfWork,
    IAIModelService aIModelService) : ICommandHandler<DeleteChatCommand>
{
    public async Task<Result> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        var botPatient = await unitOfWork.BotPatients
            .GetAsync(e => e.UserId == request.UserId && e.BotId == request.BotId, 
            includes: "Bot",
            ct: cancellationToken);

        if (botPatient == null)
            return BotPatientError.PatientNotFound;

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if (botPatient.IsUser)
            {
                var serverResult = await unitOfWork.BotPatients.DeleteBotPatientAsync(botPatient.Id, cancellationToken);

                if (serverResult.IsFailure)
                {
                    await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                    return serverResult.Error;
                }
            }
            else
            {
                var serverResult = await unitOfWork.BotMessages.DeleteBotMessageWithRelationAsync(botPatient.Id, cancellationToken);
                if (serverResult.IsFailure)
                {
                    await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                    return serverResult.Error;
                }
            }

            var result = await aIModelService.DeleteChatAsync(request.UserId, botPatient.Bot.Name, cancellationToken);

            if (result.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return result.Error;
            }

            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success();
        }
        catch
        {
            // DOTO: log error
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.BadRequest("Error.DeleteChat", "error occure while delete chat bot.");
        }
    }
}
