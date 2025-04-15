using Autine.Application.Contracts.Bots;

namespace Autine.Application.Features.Bots.Queries.GetAll;
public class GetBotsQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetBotsQuery, ICollection<BotResponse>>
{
    public async Task<Result<ICollection<BotResponse>>> Handle(GetBotsQuery request, CancellationToken cancellationToken)
    {
        var bots = await unitOfWork.Bots.GetAllAsync(e => e.CreatorId == request.UserId, ct: cancellationToken);

        if (!bots.Any())
            return BotErrors.BotNotFound;

        var response = bots.Adapt<List<BotResponse>>();  

        return response.ToList();
    }
}