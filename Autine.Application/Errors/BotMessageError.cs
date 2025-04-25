namespace Autine.Application.Errors;

public class BotMessageError
{
    public static readonly Error FailedToSendMessage
        = Error.BadRequest($"Bot.{FailedToSendMessage}", "failed to send message");
}