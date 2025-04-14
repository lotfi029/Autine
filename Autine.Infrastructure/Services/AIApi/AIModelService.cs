using Autine.Application.Contracts.Bot;
using Autine.Application.Interfaces.AIApi;

namespace Autine.Infrastructure.Services.AIApi;
public class AIModelService : IAIModelService
{
    public Task<Result> AddModelAsync(string userId, CreateBotRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
