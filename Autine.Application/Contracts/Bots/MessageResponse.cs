namespace Autine.Application.Contracts.Bots;
public record MessageResponse(
    Guid Id,
    string Content,
    DateTime Timestamp,
    MessageStatus Status,
    bool IsBot
    );
