namespace Autine.Application.Contracts.Chats;
public record ChatResponse(
    Guid Id,
    string Name,
    string UserId,
    string ProfilePic,
    DateTime CreatedAt
    );
