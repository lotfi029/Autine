using Autine.Application.Contracts.Bots;
using Autine.Application.Features.BotMessages.Commands.Send;
using Autine.Application.Features.BotMessages.Queries.GetAll;

namespace Autine.Api.Controllers;
[Route("api/[controller]/{botPatientId:guid}")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class BotMessagesController(ISender sender) : ControllerBase
{
    [HttpPost("send")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendMessage(
        [FromRoute] Guid botPatientId,
        [FromBody] MessageRequest request,
        CancellationToken cancellationToken)
    {
        string userId = User.GetUserId()!;

        var response = await sender
            .Send(new SendMessageToBotCommand(userId, botPatientId, request.Content), cancellationToken);
        
        return Ok(response);
    }
    [HttpGet("history")]
    [ProducesResponseType(typeof(IEnumerable<MessageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessageHistory(
        [FromRoute] Guid botPatientId,
        CancellationToken cancellationToken)
    {
        string userId = User.GetUserId()!;
        var response = await sender
            .Send(new GetBotMessagesQuery(userId, botPatientId), cancellationToken);

        return Ok(response);
    }
}
