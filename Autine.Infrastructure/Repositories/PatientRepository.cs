using Autine.Domain.Interfaces;

namespace Autine.Infrastructure.Repositories;
public class PatientRepository(ApplicationDbContext context) : Repository<Patient>(context), IPatientRespository
{

}