namespace Autine.Application.Features.Bots.Commands.Remove;
public class RemoveBotCommandHandler(
    IUnitOfWork unitOfWork,
    IRoleService roleService,
    IAIModelService aIModelService) : ICommandHandler<RemoveBotCommand>
{
    public async Task<Result> Handle(RemoveBotCommand request, CancellationToken cancellationToken)
    {
        var bot = await unitOfWork.Bots
            .GetAsync(
            e => !e.IsDisabled &&
            e.Id == request.BotId &&
            e.CreatedBy == request.UserId,
            ct: cancellationToken);

        if (bot is null)
            return BotErrors.BotNotFound;

        using var beginTransaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Bots
                .ExcuteUpdateAsync(
                b => b.Id == request.BotId,
                b => b.SetProperty(e => e.IsDisabled, true),
                ct: cancellationToken);

            await unitOfWork.BotPatients
                .ExcuteUpdateAsync(
                b => b.BotId == request.BotId,
                b => b.SetProperty(e => e.IsDisabled, true),
                ct: cancellationToken);

            var isAdmin = await roleService.UserIsAdminAsync(request.UserId);
            
            var aiResult = await aIModelService.RemoveModelAsync(
                request.UserId,
                bot.Name,
                isAdmin.IsSuccess,
                cancellationToken);

            if (aiResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(beginTransaction, cancellationToken);
                return aiResult;
            }
            await unitOfWork.CommitTransactionAsync(beginTransaction, cancellationToken);
            return Result.Success();
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(beginTransaction, cancellationToken);
            return Error.BadRequest("Error", "an error occure");
        }
    }
}
