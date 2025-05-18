namespace Autine.Application.Contracts.Chats;

public record UserChatResponse(
    string Id,
    string FirstName,
    string LastName,
    string Bio,
    string ProfilePic
    );