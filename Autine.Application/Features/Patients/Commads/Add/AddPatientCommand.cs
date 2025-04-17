namespace Autine.Application.Features.Patients.Commads.Add;
public record AddPatientCommand(string UserId, RegisterRequest Request) : ICommand<Guid>;
