using Autine.Application.Features.Threads.Queries.Get;
using Autine.Application.Features.Threads.Queries.GetAll;
using Autine.Application.Features.ThreadMember.Commands.Add;
using Autine.Application.Features.ThreadMember.Commands.Remove;
using Autine.Application.Contracts.Threads;
using System.Threading;
using Autine.Application.Features.ThreadMember.Queries.GetAll;

namespace Autine.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Parent.Name}, {DefaultRoles.Doctor.Name}")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class ThreadsController(ISender sender) : ControllerBase
{
    [HttpPost("{patientId:guid}/add-member")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddThreadMember([FromRoute] Guid patientId, string memberId, CancellationToken ct)
    {
        var userId = User.GetUserId()!;

        var command = new AddThreadMemberCommand(userId, patientId, memberId);

        if (userId == memberId)
            return Created();

        var result = await sender.Send(command, ct);

        return result.IsSuccess
            ? CreatedAtAction(
        nameof(GetThread),
                new { threadId = result.Value},
                null
            )
            : result.ToProblem();
    }

    [HttpDelete("{id:guid}/remove-member")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveMember([FromRoute] Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId()!;

        var command = new RemoveMemberCommand(userId, id);

        var result = await sender.Send(command, ct);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(IEnumerable<ThreadMemberResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllThreads(CancellationToken ct)
    {
        var userId = User.GetUserId()!;
        var query = new GetThreadsQuery(userId);
        var result = await sender.Send(query, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ThreadMemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetThread([FromRoute] Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId()!;
        var query = new GetThreadQuery(userId, id);
        var result = await sender.Send(query, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("{id:guid}/thread-member")]
    [ProducesResponseType(typeof(IEnumerable<ThreadMemberResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetThreadMembers([FromRoute] Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId()!;
        var query = new GetThreadMembersQuery(userId, id);
        var result = await sender.Send(query, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}