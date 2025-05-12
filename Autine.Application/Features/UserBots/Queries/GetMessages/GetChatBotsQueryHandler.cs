using Autine.Application.Contracts.UserBots;
namespace Autine.Application.Features.UserBots.Queries.GetMessages;
public class GetChatBotsQueryHandler(
    IUnitOfWork unitOfWork) : IQueryHandler<GetChatBotsQuery, List<MessageResponse>>
{
    public async Task<Result<List<MessageResponse>>> Handle(GetChatBotsQuery request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.BotPatients.GetAsync(e => e.UserId == request.UserId && e.BotId == request.BotId,ct: cancellationToken) is not { } botPatient)
            return BotErrors.BotNotFound;
        
        var messages = await unitOfWork.BotPatients.GetMessagesAsync(botPatient.Id, cancellationToken);

        if (messages is null || !messages.Any())
            return BotErrors.BotNotFound;

        var result = messages.Select(m => new MessageResponse(
            m.Id,
            m.Content,
            m.CreatedDate,
            m.Status,
            m.SenderId != null
            )).ToList();

        return result;
    }
}