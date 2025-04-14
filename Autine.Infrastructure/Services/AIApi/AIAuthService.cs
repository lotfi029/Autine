using Autine.Application.ExternalContracts;
using Autine.Application.ExternalContracts.Auth;
using Autine.Application.Interfaces.AIApi;
using Autine.Infrastructure.Abstractions;
using Microsoft.Extensions.Options;

namespace Autine.Infrastructure.Services.AIApi;
public class AIAuthService(
    IBaseService baseService,
    IOptions<ApiSettings> apiSetting) : IAIAuthService
{
    private readonly ApiSettings _apiSetting = apiSetting.Value;

    public async Task<Result> RegisterAsync(AIRegisterRequest request, CancellationToken ct = default)
        => await baseService.SendAsync(
            new Request (
                $"{_apiSetting.AIApi}/auth/user/register",
                Data: request
        ), ct);
    public async Task<Result> SupervisorAsync(AIRegisterRequest request, CancellationToken ct = default)
        => await baseService.SendAsync(
            new Request (
                $"{_apiSetting.AIApi}/auth/supervisor/register",
                Data: request
        ), ct);
}
