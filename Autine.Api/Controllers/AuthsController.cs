using Autine.Application.Contracts.Auths;
using Autine.Application.Features.Auth.ConfirmEmail;
using Autine.Application.Features.Auth.ForgotPassword;
using Autine.Application.Features.Auth.Login;
using Autine.Application.Features.Auth.ReConfirmEmail;
using Autine.Application.Features.Auth.Register;
using Autine.Application.Features.Auth.RegisterSupervisor;
using Autine.Application.Features.Auth.ResetPassword;

namespace Autine.Api.Controllers;


[Route("auths")]
[ApiController]
[Produces("application/json")]
public class AuthsController(ISender _sender) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromForm] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpPost("supervisor-register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SupervisorRegister([FromForm] CreateSupervisorRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterSupervisorCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(TokenRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateTokenCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("confirm-email")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var command = new ConfirmEmailCommand(request);

        var result = await _sender.Send(command);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("reconfirm-email")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ReConfirmEmail([FromBody] ResendConfirmEmailRequest request)
    {
        var command = new ReConfirmEmailCommand(request);

        var result = await _sender.Send(command);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var command = new ForgotPasswordCommand(request);

        var result = await _sender.Send(command);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {

        var command = new ResetPasswordCommand(request);

        var result = await _sender.Send(command);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();

    }
}
