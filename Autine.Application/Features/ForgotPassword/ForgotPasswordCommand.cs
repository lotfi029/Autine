namespace Autine.Application.Features.ForgotPassword;
public record ForgotPasswordCommand(ForgotPasswordRequest Request) : ICommand<RegisterResponse>;
