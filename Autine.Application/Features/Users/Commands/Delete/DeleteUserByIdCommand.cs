namespace Autine.Application.Features.Users.Commands.Delete;
public record DeleteUserByIdCommand(string AdminId, string UserId) : ICommand;

public class DeleteUserByIdCommandHandler(IUserService userService, IAIAuthService aIAuthService, IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserByIdCommand>
{
    public async Task<Result> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var serverResult = await userService.DeleteUserAsync(request.UserId, cancellationToken);

            if (serverResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return serverResult.Error;
            }

            var aiResult = await aIAuthService.DeleteUserAsync(
                serverResult.Value,
                request.UserId,
                Consts.FixedPassword,
                cancellationToken
                );

            if (aiResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return aiResult.Error;
            }

            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success();
        }
        catch
        {

            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.BadRequest("Error.DeleteUser", "error occure while deleting user");
        }
    }
}
