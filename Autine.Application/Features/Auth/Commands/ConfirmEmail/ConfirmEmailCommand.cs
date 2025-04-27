using Autine.Application.Contracts.Auth;

namespace Autine.Application.Features.Auth.Commands.ConfirmEmail;
public record ConfirmEmailCommand(ConfirmEmailRequest Reqeust) : ICommand;
