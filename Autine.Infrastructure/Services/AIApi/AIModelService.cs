using Autine.Application.ExternalContracts.Bots;
using Autine.Application.Interfaces.AIApi;
using Autine.Infrastructure.Abstractions;
using Microsoft.Extensions.Options;

namespace Autine.Infrastructure.Services.AIApi;
public class AIModelService(
    IBaseService baseService,
    IOptions<ApiSettings> options) : IAIModelService
{
    private readonly ApiSettings _options = options.Value;

    public async Task<Result> AddModelAsync(string userId, BotRequest request, bool isAdmin = false, CancellationToken ct = default)
    {
        if (isAdmin)
            return await baseService.SendAsync(new(
                $"{_options.AIApi}/model/admin/add?username={userId}&session_id={1}",
                Data: request
                ), ct);

        return await baseService.SendAsync(new(
                $"{_options.AIApi}/model/supervisor/add?username={userId}&session_id={1}",
                Data: request
                ), ct);
    }
    public async Task<Result> AssignModelAsync(string userId, string modelName, string patientId, CancellationToken ct = default)
    {
        var url = $"{_options.AIApi}/assign/supervisor/add?supervisor_username={userId}&user_username={patientId}&model_name={modelName}&session_id={1}";
        var response = await baseService.SendAsync(new(
            url
            ), ct);

        return response;
    }


}
