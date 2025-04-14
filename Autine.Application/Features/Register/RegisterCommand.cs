


namespace Autine.Application.Features.Register;
public record RegisterCommand(RegisterRequest Request) : ICommand<RegisterResponse>;
