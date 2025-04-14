using Autine.Application.Abstractions.Messaging;
using Autine.Application.Contracts.Auth;
using Autine.Application.Interfaces;

namespace Autine.Application.Features.Login;

public class CreateTokenCommandHandler(IAuthService _authService) : ICommandHandler<CreateTokenCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(CreateTokenCommand request, CancellationToken cancellationToken) 
        => await _authService.GetTokenAsync(request.Request, cancellationToken);
}