namespace Autine.Application.Contracts.Bots;
public record BotResponse(
    Guid Id,
    string Name,
    string Context,
    string Bio,
    DateTime CreateAt
);