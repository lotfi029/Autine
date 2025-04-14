using Autine.Application.Contracts.Bot;

namespace Autine.Application.Features.Bot.CreateBot;
public record CreateBotCommand(string UserId, CreateBotRequest Request) : ICommand;