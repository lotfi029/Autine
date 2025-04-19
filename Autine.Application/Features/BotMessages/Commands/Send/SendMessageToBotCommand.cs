using Autine.Application.Contracts.Bots;

namespace Autine.Application.Features.BotMessages.Commands.Send;
public record SendMessageToBotCommand(string UserId, Guid BotPatientId, string Content) : ICommand<MessageResponse>;
