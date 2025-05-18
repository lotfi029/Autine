
using Autine.Application.Contracts.Chats;
using Autine.Application.Contracts.UserBots;

namespace Autine.Application.Features.Messages.Queries.GetConnections;

public class GetUserConnectionsQueryHandler(IUserService userService) : IQueryHandler<GetUserConnectionsQuery, IEnumerable<UserChatResponse>>
{
    public async Task<Result<IEnumerable<UserChatResponse>>> Handle(GetUserConnectionsQuery request, CancellationToken ct)
    {
        var chats = await userService.GetAllUserChatAsync(request.UserId, ct);

        return Result.Success(chats);
    }
}
