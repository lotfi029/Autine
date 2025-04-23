using Autine.Application.Contracts.Profiles;

namespace Autine.Application.Features.Profiles.Queries;
public class GetProfileQueryHandler(IUserService userService) : IQueryHandler<GetProfileQuery, UserProfileResponse>
{
    public async Task<Result<UserProfileResponse>> Handle(GetProfileQuery request, CancellationToken ct)
        => await userService.GetProfileAsync(request.UserId, ct);
}