using Autine.Application.Contracts.Auths;

namespace Autine.Application.Features.Users.Commands.AddAdmin;
public record AddAdminCommand(string AdminId, RegisterRequest Request) : ICommand<string>;

public class AddAdminCommandHandler(IUnitOfWork unitOfWork, IAuthService authService, IAIAuthService aIAuthService) : ICommandHandler<AddAdminCommand, string>
{
    public async Task<Result<string>> Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await authService.RegisterAdminAsync(request.Request, cancellationToken);

            if (result.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return result.Error;
            }

            var externalRegisterResult = await aIAuthService.AdminAddAdmin(
                request.AdminId,
                new(
                email: request.Request.Email,
                username: result.Value,
                password: Consts.FixedPassword,
                fname: request.Request.FirstName,
                lname: request.Request.LastName,
                dateofbirth: request.Request.DateOfBirth,
                gender: request.Request.Gender
            ), cancellationToken);

            if (!externalRegisterResult.IsSuccess)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return externalRegisterResult.Error;
            }

            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success(result.Value);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.InternalServerError("Error", "An error occure while register user.");
        }

    }
}
