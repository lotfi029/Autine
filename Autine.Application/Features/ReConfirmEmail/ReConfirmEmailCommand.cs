namespace Autine.Application.Features.ReConfirmEmail;
public record ReConfirmEmailCommand(ResendConfirmEmailRequest Request) : ICommand<RegisterResponse>;
