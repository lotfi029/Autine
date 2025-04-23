namespace Autine.Application.Features.Profiles.Commands.ChangePassword;
public class ChangePasswordCommandHandler(
    IUserService userService,
    IUnitOfWork unitOfWork) : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var serverResult = await userService.ChangePasswordAsync(request.UserId, request.ChangePasswordRequest, cancellationToken);
            if (!serverResult.IsSuccess)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return serverResult;
            }

            // add AI logic here
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success();
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.InternalServerError("Error", "An error occure while change password info");
        }
    }
}
