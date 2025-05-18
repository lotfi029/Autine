using Autine.Application.Contracts.Chats;

namespace Autine.Application.Features.Messages.Queries.GetChat;

public class GetChatByIdQueryHandler(IUserService userService, IUnitOfWork unitOfWork) : IQueryHandler<GetChatByIdQuery,DetailedChatResponse>
{
    public async Task<Result<DetailedChatResponse>> Handle(GetChatByIdQuery request, CancellationToken ct)
    {
        var n = string.CompareOrdinal(request.UserId, request.ReceiverId) > 0;
        var userId = n ? request.UserId : request.ReceiverId;
        var receiverId = !n ? request.UserId : request.ReceiverId;

        if (await unitOfWork.Chats.GetAsync(e => e.UserId == receiverId && e.CreatedBy == userId, includes: "Messages", ct: ct) is not { } chat)
            return ChatErrors.ChatNotFound;

        var response = await userService.GetChatByIdAsync(request.UserId, chat.Id, ct);

        return response;
    }
}
