using Autine.Application.Contracts.Threads;

namespace Autine.Application.Features.ThreadMember.Queries.GetAll;
public record GetThreadMembersQuery(string UserId, string PatientId) : IQuery<IEnumerable<ThreadMemberResponse>>;