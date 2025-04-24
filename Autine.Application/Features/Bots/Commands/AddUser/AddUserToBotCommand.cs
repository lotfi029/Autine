namespace Autine.Application.Features.Bots.Commands.AddUser;
public record AddUserToBotCommand(string UserId, Guid BotId) : ICommand<Guid>;
