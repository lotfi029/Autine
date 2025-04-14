

namespace Autine.Application.Features.Register;
public class RegisterCommandHandler(IAuthService _authService) : ICommandHandler<RegisterCommand, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        => await _authService.RegisterAsync(request.Request, cancellationToken);
}
