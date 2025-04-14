using Autine.Application.Contracts.Bot;

namespace Autine.Application.Interfaces.AIApi;
public interface IAIModelService
{
    Task<Result> AddModelAsync(string userId, CreateBotRequest request, CancellationToken ct = default);
}
