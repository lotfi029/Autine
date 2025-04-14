namespace Autine.Application.Features.Auth.ReConfirmEmail;
public record ReConfirmEmailCommand(ResendConfirmEmailRequest Request) : ICommand<RegisterResponse>;
