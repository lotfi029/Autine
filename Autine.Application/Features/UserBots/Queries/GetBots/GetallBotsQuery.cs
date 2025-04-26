using Autine.Application.Contracts.Bots;

namespace Autine.Application.Features.UserBots.Queries.GetBots;
public record GetAllBotsQuery : IQuery<IEnumerable<PatientBotsResponse>>;


public class GetAllBotsQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetAllBotsQuery, IEnumerable<PatientBotsResponse>>
{
    public async Task<Result<IEnumerable<PatientBotsResponse>>> Handle(GetAllBotsQuery request, CancellationToken cancellationToken)
    {
        var bots = await unitOfWork.Bots.GetAllAsync(e => e.IsPublic, ct: cancellationToken);

        var response = bots.Adapt<IEnumerable<PatientBotsResponse>>();

        return Result.Success(response);
    }
}
