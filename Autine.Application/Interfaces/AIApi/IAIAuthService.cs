using Autine.Application.ExternalContracts.Auth;

namespace Autine.Application.Interfaces.AIApi;
public interface IAIAuthService
{
    Task<Result> RegisterAsync(AIRegisterRequest request, CancellationToken ct = default);
    Task<Result> SupervisorAsync(AIRegisterRequest request, CancellationToken ct = default);
    Task<Result> AddPatientAsync(string username, AIRegisterRequest request, CancellationToken ct = default);
    Task<Result> RemovePatientAsync(string username, string user_username, CancellationToken ct = default);
}
