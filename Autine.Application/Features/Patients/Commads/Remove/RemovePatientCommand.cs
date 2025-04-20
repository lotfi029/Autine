namespace Autine.Application.Features.Patients.Commads.Remove;
public record RemovePatientCommand(string UserId, Guid Id) : ICommand;
