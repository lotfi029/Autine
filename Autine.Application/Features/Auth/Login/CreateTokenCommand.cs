using Autine.Application.Abstractions.Messaging;
using Autine.Application.Contracts.Auth;

namespace Autine.Application.Features.Auth.Login;
public record CreateTokenCommand(TokenRequest Request) : ICommand<AuthResponse>;
