using Autine.Application.Contracts.Auths;

namespace Autine.Application.Features.Auth.Commands.RefreshTokens;

public class RefreshTokenCommandHandler(IAuthService authService) : ICommandHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        => await authService.GetRefreshTokenAsync(request.RefreshToken.Token, request.RefreshToken.RefreshToken, cancellationToken);
}
