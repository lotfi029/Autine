using Autine.Application.Contracts.Thread;

namespace Autine.Application.Features.Thread.Queries.GetAll;
public record GetThreadsQuery(string UserId) : IQuery<IEnumerable<ThreadResponse>>;
