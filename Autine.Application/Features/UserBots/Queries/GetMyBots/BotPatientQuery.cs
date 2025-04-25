using Autine.Application.Contracts.Bots;

namespace Autine.Application.Features.UserBots.Queries.GetMyBots;
public record BotPatientQuery(string UserId) : IQuery<IEnumerable<BotPatientsResponse>>;

public class BotPatientCommandHandler : IQueryHandler<BotPatientQuery, IEnumerable<BotPatientsResponse>>
{
    public Task<Result<IEnumerable<BotPatientsResponse>>> Handle(BotPatientQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
