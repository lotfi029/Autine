namespace Autine.Application.Features.Auth.ForgotPassword;
public record ForgotPasswordCommand(ForgotPasswordRequest Request) : ICommand<RegisterResponse>;
