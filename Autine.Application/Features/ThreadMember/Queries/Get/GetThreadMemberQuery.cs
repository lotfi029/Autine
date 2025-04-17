using Autine.Application.Contracts.Thread;

namespace Autine.Application.Features.ThreadMember.Queries.Get;
public record GetThreadMemberQuery(Guid Id) : IQuery<ThreadMemberResponse>;
