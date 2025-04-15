using Autine.Application.Contracts.Patient;
using Autine.Application.Features.Patient.Commads.Add;
using Autine.Application.Features.Patient.Queries.GetAll;

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
        var command = new AddPatientCommand(userId,request);
        var result = await sender.Send(command, ct);
        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<PatientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatients(CancellationToken ct)
    {
        var userId = User.GetUserId()!;
        
        var query = new GetPatientsQuery(userId);

        var result = await sender.Send(query, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
