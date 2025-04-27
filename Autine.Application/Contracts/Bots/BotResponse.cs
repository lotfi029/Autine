namespace Autine.Application.Contracts.Bots;
public record BotResponse(
    Guid Id,
    string Name,
    string Bio,
    string Image,
    IList<BotPatientsResponse> Patients
);
