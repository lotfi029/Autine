
using Autine.Application.Features.Patient.Add;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Security.Claims;

namespace Autine.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Doctor.Name}, {DefaultRoles.Parent.Name}")]
public class PatientsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddPatient([FromForm] RegisterRequest request, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)! ?? string.Empty;
        var command = new AddPatientCommand(userId,request);
        var result = await sender.Send(command, ct);
        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
}
