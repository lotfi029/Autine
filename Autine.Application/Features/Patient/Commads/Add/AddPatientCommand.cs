namespace Autine.Application.Features.Patient.Commads.Add;
public record AddPatientCommand(string UserId, RegisterRequest Request) : ICommand;
