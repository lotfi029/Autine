namespace Autine.Application.Features.Profiles.Commands.ChangePassword;
public class ChangePasswordCommandHandler(
    IAccountService accountService) : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        { 
            var serverResult = await accountService.ChangePasswordAsync(request.UserId, request.ChangePasswordRequest, cancellationToken);
            
            if (!serverResult.IsSuccess)
                return serverResult;

            return Result.Success();
        }
        catch
        {
            // TODO: Log error
            return Error.InternalServerError("Error.ChangePassword", "An error occure while change password info");
        }
    }
}
