using Autine.Application.Contracts.Auth;

namespace Autine.Application.Features.Auth.Commands.Register;
public record RegisterCommand(RegisterRequest Request) : ICommand<RegisterResponse>;
