using Autine.Application.Contracts.Bots;

namespace Autine.Application.Features.Patients.Queries.GetBots;
public record GetPatientBotsQuery(string UserId, Guid PatientId) : IQuery<IEnumerable<PatientBotResponse>>;