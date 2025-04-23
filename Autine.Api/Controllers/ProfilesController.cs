using Autine.Application.Contracts.Files;
using Autine.Application.Contracts.Profiles;

namespace Autine.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public class ProfilesController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    public Task<IActionResult> GetProfile(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> UpdateProfile(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> ChangePassword(CancellationToken ct = default)
    {
        throw new NotImplementedException();
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
