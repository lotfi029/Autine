using Autine.Application.Contracts.Thread;

namespace Autine.Application.Features.ThreadMember.Queries.Get;
public record GetThreadMemberQuery(string UserId, Guid ThreadId) : IQuery<ThreadResponse>;
