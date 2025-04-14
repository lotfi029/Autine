using Autine.Domain.Interfaces;

namespace Autine.Infrastructure.Repositories;
public class PatientRepository(ApplicationDbContext context) : Repository<PatientSupervisor>(context), IPatientRespository
{

}