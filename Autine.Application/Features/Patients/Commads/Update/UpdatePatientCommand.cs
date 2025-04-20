using Autine.Application.Contracts.Patients;

namespace Autine.Application.Features.Patients.Commads.Update;
public record UpdatePatientCommand(string UserId, Guid PatientId, UpdateUserRequest UpdateRequest) : ICommand;
