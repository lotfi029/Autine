
using Autine.Application.IServices;
using Autine.Application.IServices.AIApi;

namespace Autine.Application.Features.Profiles.Commands.Delete;
public class DeleteProfileCommandHandler(
    IAIAuthService aIAuthService,
    IUserService userService,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteProfileCommand>
{
    public async Task<Result> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var serverResult = await userService.DeleteUserAsync(request.UserId,cancellationToken, transaction);

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
