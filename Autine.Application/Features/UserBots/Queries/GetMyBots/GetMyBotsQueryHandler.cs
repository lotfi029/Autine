using Autine.Application.Contracts.Bots;
using Autine.Application.IServices;

namespace Autine.Application.Features.UserBots.Queries.GetMyBots;

public class GetMyBotsQueryHandler(IBotService botService) : IQueryHandler<GetMyBotsQuery, IEnumerable<PatientBotsResponse>>
{
    public async Task<Result<IEnumerable<PatientBotsResponse>>> Handle(GetMyBotsQuery request, CancellationToken cancellationToken)
    {
        var response = await botService.GetPatientBotsAsync(request.UserId, cancellationToken);

        return Result.Success(response);
    }
}
