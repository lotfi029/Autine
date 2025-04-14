

namespace Autine.Application.Features.ReConfirmEmail;
public class ReConfirmEmailCommandHandler(IAuthService _authService) : ICommandHandler<ReConfirmEmailCommand, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Handle(ReConfirmEmailCommand request, CancellationToken cancellationToken)
        => await _authService.ReConfirmEmailAsync(request.Request);
}
