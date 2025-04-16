using Autine.Application.Contracts.Bots;

namespace Autine.Application.Features.Bots.Queries.GetById;
public class GetBotByIdQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetBotByIdQuery, BotResponse>
{
    public async Task<Result<BotResponse>> Handle(GetBotByIdQuery request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Bots.FindByIdAsync(cancellationToken, [request.BotId]) is not { } bot)
            return BotErrors.BotNotFound;

        if (bot.CreatedBy != request.UserId)
            return BotErrors.BotNotFound;

        var response = bot.Adapt<BotResponse>();

        return response;
    }
}
