namespace Autine.Domain.Interfaces;
public interface IPatientRespository : IRepository<Patient>
{
    Task<IEnumerable<Patient>> GetAllThreads(string userId, CancellationToken ct = default);
}
