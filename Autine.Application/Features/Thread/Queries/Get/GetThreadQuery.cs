using Autine.Application.Contracts.Thread;

namespace Autine.Application.Features.Thread.Queries.Get;
public record GetThreadQuery(string UserId, Guid Id) : IQuery<ThreadResponse>;
