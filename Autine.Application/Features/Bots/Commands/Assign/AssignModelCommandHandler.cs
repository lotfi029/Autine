namespace Autine.Application.Features.Bots.Commands.Assign;
public class AssignModelCommandHandler(
    IUnitOfWork unitOfWork,
    IUserService userService,
    IAIModelService aIModelService) : ICommandHandler<AssignModelCommand>
{
    public async Task<Result> Handle(AssignModelCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Bots.FindByIdAsync(cancellationToken, [request.BotId]) is not { } bot)
            return BotErrors.BotNotFound;

        if (bot.CreatedBy != request.UserId)
            return BotErrors.BotNotFound;

        if (!await userService.CheckUserExist(request.PatientId, cancellationToken))
            return UserErrors.UserNotFound;

        var result = await aIModelService.AssignModelAsync(request.UserId, bot.Name, request.PatientId, cancellationToken);

        if (result.IsFailure)
            return result;

        await unitOfWork.BotPatients.AddAsync(new()
        {
            BotId = request.BotId,
            PatientId = request.PatientId
        }, cancellationToken);

        return Result.Success();
    }
}
