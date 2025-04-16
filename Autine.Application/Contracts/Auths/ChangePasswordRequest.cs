namespace Autine.Application.Contracts.Auths;

public record ChangePasswordRequest
(
    string CurrentPassword,
    string NewPassword
);