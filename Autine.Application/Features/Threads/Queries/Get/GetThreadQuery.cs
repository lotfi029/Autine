using Autine.Application.Contracts.Threads;

namespace Autine.Application.Features.Threads.Queries.Get;
public record GetThreadQuery(string UserId, Guid Id) : IQuery<ThreadResponse>;
