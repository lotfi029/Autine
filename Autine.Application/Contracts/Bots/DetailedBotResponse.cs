namespace Autine.Application.Contracts.Bots;

public record DetailedBotResponse(
    Guid Id,
    string Name,
    string Bio,
    string Context,
    DateTime CreateAt,
    IList<BotPatientsResponse> Patients
    );