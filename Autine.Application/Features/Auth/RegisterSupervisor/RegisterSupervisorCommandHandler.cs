namespace Autine.Application.Features.Auth.RegisterSupervisor;
public class RegisterSupervisorCommandHandler(
    IAuthService authService, 
    IAIAuthService aIAuthService) : ICommandHandler<RegisterSupervisorCommand, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterSupervisorCommand request, CancellationToken cancellationToken)
    {
        // TODO:



        var authResult = await authService.RegisterSupervisorAsync(request.Request, cancellationToken);

        if (authResult.IsFailure)
            return authResult.Error;

        var aiResult = await aIAuthService.SupervisorAsync(new(
            request.Request.Email,
            authResult.Value.UserId,
            request.Request.Password,
            request.Request.FirstName,
            request.Request.LastName,
            request.Request.DateOfBirth,
            request.Request.Gender
            ), cancellationToken);

        if (aiResult.IsFailure)
            return aiResult.Error;

        return authResult.Value;
    }
}