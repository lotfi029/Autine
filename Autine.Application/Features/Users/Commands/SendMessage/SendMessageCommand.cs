using Autine.Application.Contracts.UserBots;

namespace Autine.Application.Features.Users.Commands.SendMessage;
public record SendMessageCommand(string UserId, string RecieverId, string Content) : ICommand<MessageResponse>;

public class SendMessageCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<SendMessageCommand, MessageResponse>
{
    public Task<Result<MessageResponse>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {

        }
        catch
        {

        }
    }
}
