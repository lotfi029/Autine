using Autine.Application.Contracts.Chats;

namespace Autine.Application.Features.Messages.Queries.GetChats;

public class GetChatsQueryHandler(IUserService userService) : IQueryHandler<GetChatsQuery, IEnumerable<ChatResponse>>
{
    public async Task<Result<IEnumerable<ChatResponse>>> Handle(GetChatsQuery request, CancellationToken ct)
    {
        var response = await userService.GetAllChatsAsync(request.UserId, ct);

        if (response == null)
            return Result.Success(Enumerable.Empty<ChatResponse>());

        return Result.Success(response);
    }
}