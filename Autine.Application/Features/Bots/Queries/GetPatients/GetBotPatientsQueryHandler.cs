using Autine.Application.Contracts.Bots;

namespace Autine.Application.Features.Bots.Queries.GetPatients;
public class GetBotPatientsQueryHandler(
    IUnitOfWork unitOfWork,
    IUserService userService) : IQueryHandler<GetBotPatientsQuery, IEnumerable<BotPatientResponse>>
{
    public async Task<Result<IEnumerable<BotPatientResponse>>> Handle(GetBotPatientsQuery request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Bots.FindByIdAsync(cancellationToken, [request.BotId]) is not { } bot)
            return BotErrors.BotNotFound;

        if (bot.CreatedBy!= request.UserId || bot.IsDisabled)
            return BotErrors.BotNotFound;

        var patients = await userService.GetBotPatientAsync(request.BotId, cancellationToken);

        return Result.Success(patients);
    }
}
