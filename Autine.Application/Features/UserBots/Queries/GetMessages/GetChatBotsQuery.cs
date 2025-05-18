using Autine.Application.Contracts.Bots;
using Autine.Application.Contracts.Chats;

namespace Autine.Application.Features.UserBots.Queries.GetMessages;
public record GetChatBotsQuery(string UserId, Guid BotId) : IQuery<DetailedChatBotResponse>;