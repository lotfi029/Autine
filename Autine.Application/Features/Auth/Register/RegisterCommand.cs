using Autine.Application.Contracts.Auths;

namespace Autine.Application.Features.Auth.Register;
public record RegisterCommand(RegisterRequest Request) : ICommand<RegisterResponse>;
