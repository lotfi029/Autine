namespace Autine.Application.Contracts.Auths;

public record ConfirmEmailRequest(string UserId, string Code);
