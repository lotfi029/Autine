using Autine.Application.Contracts.Bots;

namespace Autine.Application.Features.UserBots.Queries.GetMyBots;
public record BotPatientQuery(string UserId) : IQuery<IEnumerable<BotPatientsResponse>>;



public class BotPatientCommandHandler(IUnitOfWork unitOfWork) : IQueryHandler<BotPatientQuery, IEnumerable<BotPatientsResponse>>
{
    public async Task<Result<IEnumerable<BotPatientsResponse>>> Handle(BotPatientQuery request, CancellationToken cancellationToken)
    {
        var bots = await unitOfWork.BotPatients.GetAllAsync(e => e.UserId == request.UserId, ct: cancellationToken);

        var response = bots.Select(x => new BotPatientsResponse(Id: x.Id, Name: "", CreateAt: x.CreatedAt, ProfilePic: "not implemented yet")); 

        return Result.Success(response);
    }
}
