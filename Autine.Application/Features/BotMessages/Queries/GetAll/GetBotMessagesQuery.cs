using Autine.Application.Contracts.UserBots;

namespace Autine.Application.Features.BotMessages.Queries.GetAll;
public record GetBotMessagesQuery(string UserId, Guid BotPatientId) : IQuery<List<MessageResponse>>;
public class GetBotMessagesQueriesHandler(
    IUnitOfWork unitOfWork) : IQueryHandler<GetBotMessagesQuery, List<MessageResponse>>
{
    public async Task<Result<List<MessageResponse>>> Handle(GetBotMessagesQuery request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.BotPatients.FindByIdAsync(cancellationToken, [request.BotPatientId]) is not { } botPatient)
            return BotErrors.BotNotFound;
        
        var messages = await unitOfWork.BotPatients.GetMessagesAsync(request.BotPatientId, cancellationToken);

        if (messages is null || !messages.Any())
            return BotErrors.BotNotFound;

        var result = messages.OrderBy(e => e.Message.CreatedDate).Select(m => new MessageResponse(
            m.Id,
            m.Message.Content,
            m.Message.CreatedDate,
            m.Message.Status,
            m.Message.SenderId == null
            ));


        return result.ToList();
    }
}