namespace Autine.Application.Contracts.Auths;

public record ResetPasswordRequest (
    string UserId,
    string Code,
    string Password
);
