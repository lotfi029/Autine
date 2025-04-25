using Autine.Application.Contracts.Bots;

namespace Autine.Application.Features.Bots.Queries.GetById;
public class GetBotByIdQueryHandler(
    IUnitOfWork unitOfWork,
    IPatientService patientService) : IQueryHandler<GetBotByIdQuery, DetailedBotResponse>
{
    public async Task<Result<DetailedBotResponse>> Handle(GetBotByIdQuery request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Bots.FindByIdAsync(cancellationToken, [request.BotId]) is not { } bot)
            return BotErrors.BotNotFound;

        if (bot.CreatedBy != request.UserId)
            return BotErrors.BotNotFound;

        var patients = await patientService.GetBotPatientAsync(request.BotId, cancellationToken);

        var response = (bot, patients).Adapt<DetailedBotResponse>();

        return response;
    }
}
