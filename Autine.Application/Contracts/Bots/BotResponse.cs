namespace Autine.Application.Contracts.Bots;
public record BotResponse(
    Guid Id,
    string Name,
    string Bio,
    DateTime CreateAt,
    IList<BotPatientsResponse> Patients
);

public record BotPatientsResponse(
    Guid Id,
    string Name,
    DateTime CreateAt,
    string ProfilePic
);

public record PatientBotsResponse(
    Guid Id,
    string Name,
    string ProfilePic,
    DateTime CreateAt
);