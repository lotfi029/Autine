namespace Autine.Application.Contracts.Thread;

public record ThreadMemberResponse(
    Guid Id,
    string UserId, 
    DateTime CraetedAt
    );