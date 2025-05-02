using Autine.Application.Contracts.Chats;
using Autine.Application.Contracts.UserBots;

namespace Autine.Application.Features.Messages.Queries.GetChat;

public class GetChatByIdQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetChatByIdQuery, DetailedChatResponse>
{
    public async Task<Result<DetailedChatResponse>> Handle(GetChatByIdQuery request, CancellationToken ct)
    {
        if (await unitOfWork.Chats.GetAsync(e => e.Id == request.ChatId, includes: "Messages", ct: ct) is not { } chat)
            return ChatErrors.ChatNotFound;

        if (request.UserId == chat.UserId || request.UserId == chat.CreatedBy)
            return ChatErrors.UserNotExist;

        var messages = new DetailedChatResponse(
            chat.Id,
            chat.UserId.Equals(request.UserId, StringComparison.OrdinalIgnoreCase) ? chat.CreatedBy : chat.UserId,
            chat.CreatedAt,
            chat.Messages.Select(m => new MessageResponse(
                m.Id, 
                m.Content, 
                m.CreatedDate, 
                m.Status, 
                request.UserId == m.SenderId
                )) ?? []
            );

        return Result.Success(messages);
    }
}
