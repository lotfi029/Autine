namespace Autine.Application.Errors;

public class ChatErrors
{
    public static readonly Error UserNotExist
        = Error.NotFound($"Chat.{UserNotExist}", "This user not exist");

    public static readonly Error ChatNotFound
        = Error.NotFound($"Chat.{ChatNotFound}", "chat not found");

}