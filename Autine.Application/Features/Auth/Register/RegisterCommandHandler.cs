using Autine.Application.Interfaces.AIApi;

namespace Autine.Application.Features.Auth.Register;
public class RegisterCommandHandler(
    IAuthService _authService,
    IAIAuthService _aIAuthService) : ICommandHandler<RegisterCommand, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    { 

        var result = await _authService.RegisterAsync(request.Request, cancellationToken);
        
        if (!result.IsSuccess)
            return result.Error;
        var externalRegisterResult = await _aIAuthService.RegisterAsync(new(
        
            request.Request.Email,
            result.Value.UserId,
            request.Request.Password,
            request.Request.FirstName,
            request.Request.LastName,
            request.Request.DateOfBirth,
            request.Request.Gender
        ), cancellationToken);

        if (!externalRegisterResult.IsSuccess)
            return externalRegisterResult.Error;


        //TODO: Handle the case when the external registration fails

        return result.Value;
    }
}
