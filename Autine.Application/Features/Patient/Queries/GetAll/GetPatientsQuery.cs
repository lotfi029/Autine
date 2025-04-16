using Autine.Application.Contracts.Patient;

namespace Autine.Application.Features.Patient.Queries.GetAll;
public record GetPatientsQuery(string UserId) : IQuery<ICollection<PatientResponse>>;
