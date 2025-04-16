namespace Autine.Application.Features.ThreadMember.Commands.Add;
public record AddThreadMemberCommand(string UserId, Guid PatientId, string MemberId) : ICommand<Guid>;