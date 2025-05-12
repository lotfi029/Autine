namespace Autine.Application.Contracts.UserBots;
public record MessageResponse(
    Guid Id,
    string Content,
    DateTime Timestamp,
    MessageStatus Status,
    bool Direction
    );
public record ChatResponse(
    Guid Id,
    string Name,
    string ProfilePic,
    DateTime CreateAt,
    IList<MessageResponse> Messages
    );