
using Autine.Application.Contracts.Chats;

namespace Autine.Application.Features.Messages.Queries.GetConnections;
public record GetUserConnectionsQuery(string UserId) : IQuery<IEnumerable<UserChatResponse>>;
