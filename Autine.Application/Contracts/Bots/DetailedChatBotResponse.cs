using Autine.Application.Contracts.UserBots;

namespace Autine.Application.Contracts.Bots;

public record DetailedChatBotResponse(
    Guid Id,
    string Name,
    string ProfilePic,
    DateTime CreateAt,
    IList<MessageResponse> Messages
    );