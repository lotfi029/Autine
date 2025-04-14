namespace Autine.Application.Contracts.Bot;

public record CreateBotRequest(
    string Name,
    string Context,
    string Bio
);
