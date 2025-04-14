namespace Autine.Application.Features.Auth.Register;
public record RegisterCommand(RegisterRequest Request) : ICommand<RegisterResponse>;
