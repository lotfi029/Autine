namespace Autine.Application.Features.Bots.Commands.Assign;
public record AssignModelCommand(string UserId, Guid PatientId, Guid BotId) : ICommand;