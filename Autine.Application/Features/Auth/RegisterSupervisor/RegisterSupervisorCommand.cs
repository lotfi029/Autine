namespace Autine.Application.Features.Auth.RegisterSupervisor;
public record RegisterSupervisorCommand(CreateSupervisorRequest Request) : ICommand<RegisterResponse>;
