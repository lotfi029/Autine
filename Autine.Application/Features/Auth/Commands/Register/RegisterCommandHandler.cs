namespace Autine.Application.Features.Auth.Commands.Register;
public class RegisterCommandHandler(
    IAuthService _authService,
    IAIAuthService _aIAuthService,
    IUnitOfWork unitOfWork) : ICommandHandler<RegisterCommand, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    { 

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await _authService.RegisterAsync(request.Request, cancellationToken);
        
            if (!result.IsSuccess)
                return result.Error;

            var externalRegisterResult = await _aIAuthService.RegisterAsync(new(
                request.Request.Email,
                result.Value.UserId,
                Consts.FixedPassword,
                request.Request.FirstName,
                request.Request.LastName,
                request.Request.DateOfBirth,
                request.Request.Gender
            ), cancellationToken);

            if (!externalRegisterResult.IsSuccess)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return externalRegisterResult.Error; 
            }

            var response = new RegisterResponse(result.Value.Code, result.Value.UserId);
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success(response);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.InternalServerError("Error", "An error occure while register user.");
        }
    }
}
