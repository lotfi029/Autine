namespace Autine.Application.Features.ThreadMember.Commands.Add;
public record AddThreadMemberCommand(string UserId, string PatientId, string MemberId) : ICommand<Guid>;