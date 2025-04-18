namespace Autine.Application.Contracts.Bots;

public record CreateBotRequest(
    string Name,
    string Context,
    string Bio,
    Guid[]? PatientIds
);
