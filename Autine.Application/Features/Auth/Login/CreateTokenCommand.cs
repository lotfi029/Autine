using Autine.Application.Abstractions.Messaging;
using Autine.Application.Contracts.Auths;

namespace Autine.Application.Features.Auth.Login;
public record CreateTokenCommand(TokenRequest Request) : ICommand<AuthResponse>;
