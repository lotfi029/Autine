namespace Autine.Application.Contracts.Bots;
public record BotResponse(
    Guid Id,
    string Name,
    string Context,
    string Bio,
    DateTime CreateAt,
    List<BotPatientResponse> Patients
);

public record BotPatientResponse(
    Guid Id,
    string Name,
    DateTime CreateAt,
    string ProfilePic
);