namespace Autine.Application.Features.ConfirmEmail;
public class ConfirmEmailCommandHandler(IAuthService _authService) : ICommandHandler<ConfirmEmailCommand>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        => await _authService.ConfirmEmailAsync(request.Reqeust);
}
