

namespace Autine.Application.Features.Auth.RegisterSupervisor;
public class RegisterSupervisorCommandHandler(
    IUnitOfWork unitOfWork,
    IAuthService authService, 
    IAIAuthService aIAuthService) : ICommandHandler<RegisterSupervisorCommand, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterSupervisorCommand request, CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var authResult = await authService.RegisterSupervisorAsync(request.Request, cancellationToken);

            if (authResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return authResult.Error;
            }

            var aiResult = await aIAuthService.SupervisorAsync(new(
                request.Request.Email,
                authResult.Value.UserId,
                Consts.FixedPassword,
                request.Request.FirstName,
                request.Request.LastName,
                request.Request.DateOfBirth,
                request.Request.Gender
                ), cancellationToken);

            if (aiResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return aiResult.Error;
            }

            var response = new RegisterResponse(authResult.Value.Code, authResult.Value.UserId);
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return response;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.InternalServerError("Error", "An error occure while register user.");
        }
    }
}