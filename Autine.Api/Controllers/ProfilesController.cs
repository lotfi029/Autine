using Autine.Application.Contracts.Auths;
using Autine.Application.Contracts.Files;
using Autine.Application.Contracts.Profiles;
using Autine.Application.Features.Profiles.Commands.ChangePassword;
using Autine.Application.Features.Profiles.Commands.Update;
using Autine.Application.Features.Profiles.Queries;

namespace Autine.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public class ProfilesController(ISender sender) : ControllerBase
{
    [HttpGet("")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile(CancellationToken ct = default)
    {
        var userId = User.GetUserId()!;

        var query = new GetProfileQuery(userId);
        var response = await sender.Send(query, ct);
        return response.IsSuccess
            ? Ok(response.Value)
            : response.ToProblem();
    }

    [HttpPut("")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest request,CancellationToken ct = default)
    {
        var userId = User.GetUserId()!;

        var command = new UpdateProfileCommand(userId, request);
        var result = await sender.Send(command,ct);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request,CancellationToken ct = default)
    {
        var userId = User.GetUserId()!;

        var command = new ChangePasswordCommand(userId, request);
        var result = await sender.Send(command, ct);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("change-profile-picture")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> ChangeProfilePicture(ProfilePictureRequest file, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> DeleteProfile(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

}
