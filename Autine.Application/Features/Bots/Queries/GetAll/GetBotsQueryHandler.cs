using Autine.Application.Contracts.Bots;

namespace Autine.Application.Features.Bots.Queries.GetAll;
public class GetBotsQueryHandler(
    IUnitOfWork unitOfWork,
    IUserService userService) : IQueryHandler<GetBotsQuery, ICollection<BotResponse>>
{
    public async Task<Result<ICollection<BotResponse>>> Handle(GetBotsQuery request, CancellationToken cancellationToken)
    {
        var bots = await unitOfWork.Bots.GetAllAsync(
            e => e.CreatedBy == request.UserId, 
            includes: "BotPatients",
            ct: cancellationToken);

        var response = new List<BotResponse>();
        foreach (var b in bots) {
            IEnumerable<BotPatientResponse> botPatients;
            if (b.BotPatients is not null) 
            {
                botPatients = await userService.GetBotPatientAsync([..b.BotPatients.Select(e => e.PatientId)], cancellationToken);
            }
            else
            {
                botPatients = [];
            }
            var bot = new BotResponse(
                Id: b.Id,
                Name: b.Name,
                Context: b.Context,
                Bio: b.Bio,
                CreateAt: b.CreatedAt,
                Patients: botPatients?.ToList() ?? []
                );

            response.Add(bot);

        }


        return response.ToList();
    }
}