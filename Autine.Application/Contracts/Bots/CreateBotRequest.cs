namespace Autine.Application.Contracts.Bots;

public record CreateBotRequest(
    string Name,
    string Context,
    string Bio,
    IFormFile? Image,
    IList<string>? PatientIds
);
