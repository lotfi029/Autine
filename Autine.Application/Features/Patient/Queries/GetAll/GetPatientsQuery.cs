using Autine.Application.Contracts.Patient;

namespace Autine.Application.Features.Patient.Queries.GetAll;
public record GetPatientsQuery(string userId) : IQuery<ICollection<PatientResponse>>;
