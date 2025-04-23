using Autine.Application.Contracts.Profiles;

namespace Autine.Application.Features.Patients.Commads.Update;
public record UpdatePatientCommand(string UserId, Guid PatientId, UpdateUserProfileRequest UpdateRequest) : ICommand;
