using Autine.Application.Contracts.Auths;
using Autine.Application.Contracts.Bots;
using Autine.Application.Contracts.Patients;
using Autine.Application.Features.Patients.Commads.Add;
using Autine.Application.Features.Patients.Queries.Get;
using Autine.Application.Features.Patients.Queries.GetAll;
using Autine.Application.Features.Patients.Queries.GetBots;

namespace Autine.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Doctor.Name}, {DefaultRoles.Parent.Name}")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class PatientsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddPatient([FromForm] RegisterRequest request, CancellationToken ct)
    {
        var userId = User.GetUserId()!;
        var command = new AddPatientCommand(userId, request);
        var result = await sender.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(
                nameof(GetPatientById),
                new { id = result.Value },
                null!
                )
            : result.ToProblem();
    }
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientById([FromRoute] Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId()!;
        var query = new GetPatientQuery(userId, id);
        var result = await sender.Send(query, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("my-patient")]
    [ProducesResponseType(typeof(ICollection<PatientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyPatients(CancellationToken ct)
    {
        var userId = User.GetUserId()!;

        var query = new GetPatientsQuery(userId, false);

        var result = await sender.Send(query, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("follow-patient")]
    [ProducesResponseType(typeof(ICollection<PatientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFollowPatients(CancellationToken ct)
    {
        var userId = User.GetUserId()!;

        var query = new GetPatientsQuery(userId, true);

        var result = await sender.Send(query, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("{patientId:guid}/patient-bot")]
    [ProducesResponseType(typeof(IEnumerable<PatientBotResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientBots([FromRoute] Guid patientId, CancellationToken ct)
    {
        var userId = User.GetUserId()!;
        var query = new GetPatientBotsQuery(userId, patientId);
        var result = await sender.Send(query, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}