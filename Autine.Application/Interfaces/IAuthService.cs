using Autine.Application.Contracts.Auths;

namespace Autine.Application.Interfaces;
public interface IAuthService
{
    Task<Result<InternalRegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<InternalRegisterResponse>> RegisterSupervisorAsync(CreateSupervisorRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> RegisterPatient(RegisterRequest request, CancellationToken ct = default);
    Task<Result<AuthResponse>> GetTokenAsync(TokenRequest loginRequest, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result<RegisterResponse>> ReConfirmEmailAsync(ResendConfirmEmailRequest request);
    Task<Result<RegisterResponse>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
}
