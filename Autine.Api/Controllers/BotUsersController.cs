using Autine.Application.Contracts.UserBots;
using Autine.Application.Features.UserBots.Commands.Send;
using Autine.Application.Features.UserBots.Queries.GetAll;
using Autine.Application.Features.UserBots.Queries.GetMyBots;

namespace Autine.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class BotUsersController(ISender sender) : ControllerBase
{
    [HttpPost("{botPatientId:guid}/send")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendMessage(
        [FromRoute] Guid botPatientId,
        [FromBody] MessageRequest request,
        CancellationToken cancellationToken)
    {
        string userId = User.GetUserId()!;

        var command = new SendMessageToBotCommand(userId, botPatientId, request.Content);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("{botPatientId}/history")]
    [ProducesResponseType(typeof(IEnumerable<MessageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessageHistory(
        [FromRoute] Guid botPatientId,
        CancellationToken cancellationToken)
    {
        string userId = User.GetUserId()!;
        var response = await sender
            .Send(new GetBotMessagesQuery(userId, botPatientId), cancellationToken);

        return response.IsSuccess
            ? Ok(response.Value)
            : response.ToProblem();
    }
    [HttpGet("my-bots")]
    public async Task<IActionResult> GetMyBots(CancellationToken ct = default)
    {
        var userId = User.GetUserId()!;
        var query = new BotPatientQuery(userId);
        var response = await sender.Send(query, ct);

        return response.IsSuccess
            ? Ok(response.Value)
            : response.ToProblem();
    }
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    [HttpGet("")]
    public Task<IActionResult> GetBots(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
