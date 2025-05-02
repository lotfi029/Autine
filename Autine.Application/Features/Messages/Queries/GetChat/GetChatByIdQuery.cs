using Autine.Application.Contracts.Chats;

namespace Autine.Application.Features.Messages.Queries.GetChat;
public record GetChatByIdQuery(string UserId, Guid ChatId) : IQuery<DetailedChatResponse>;


