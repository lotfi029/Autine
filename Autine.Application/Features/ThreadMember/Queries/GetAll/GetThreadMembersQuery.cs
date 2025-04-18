using Autine.Application.Contracts.Threads;

namespace Autine.Application.Features.ThreadMember.Queries.GetAll;
public record GetThreadMembersQuery(string UserId, Guid PatientId) : IQuery<IEnumerable<ThreadMemberResponse>>;