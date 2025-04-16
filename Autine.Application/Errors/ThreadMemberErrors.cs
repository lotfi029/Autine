namespace Autine.Application.Errors;

public class ThreadMemberErrors
{
    public static readonly Error ThreadMemberNotFound
        = Error.NotFound($"ThreadMember.{nameof(ThreadMemberNotFound)}", "Member not found");
    public static readonly Error ThreadMemberAlreadyExists
        = Error.Conflict($"ThreadMember.{nameof(ThreadMemberAlreadyExists)}", "Thread member already exists");
}