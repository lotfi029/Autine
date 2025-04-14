using Autine.Application.Interfaces.AIApi;

namespace Autine.Application.Features.Register;
public class RegisterCommandHandler(
    IAuthService _authService,
    IAIAuthService _aIAuthService) : ICommandHandler<RegisterCommand, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    { 
        var externalRegisterResult = await _aIAuthService.RegisterAsync(new(
        
            request.Request.Email,
            request.Request.UserName,
            request.Request.Password,
            request.Request.FirstName,
            request.Request.LastName,
            request.Request.DateOfBirth,
            request.Request.Gender
        ), cancellationToken);

        if (!externalRegisterResult.IsSuccess)
            return externalRegisterResult.Error;

        var result = await _authService.RegisterAsync(request.Request, cancellationToken);
        
        if (!result.IsSuccess)
            return result.Error;


        //TODO: Handle the case when the external registration fails

        return result.Value;
    }
}
