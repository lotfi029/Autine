using Autine.Application.Features.FIles.Queries;
using Autine.Application.Interfaces;

namespace Autine.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilesController(ISender sender) : ControllerBase
{

    [HttpGet("image/{imageName}")]
    public async Task<IActionResult> GetImage(string imageName, CancellationToken cancellationToken)
    {
        var query = new GetImageQuery(imageName);
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? File(result.Value.stream!, result.Value.contentType!, result.Value.fileName)
            : result.ToProblem();
    }
}
