using Autine.Application.ExternalContracts.Bots;

namespace Autine.Application.Interfaces.AIApi;
public interface IAIModelService
{
    Task<Result> AddModelAsync(string userId, ModelRequest request, bool isAdmin = false, CancellationToken ct = default);
    Task<Result> AssignModelAsync(string userId, string modelName, string patientId, CancellationToken ct = default);
    Task<Result<ModelMessageResponse>> SendMessageToModelAsync(string userId, string modelName, string message, CancellationToken ct = default);
}
