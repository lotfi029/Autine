namespace Autine.Domain.Interfaces;
public interface IPatientRespository : IRepository<Patient>
{
    Task<IEnumerable<Patient>> ArePatientsAsync(Guid[] ids, CancellationToken ct = default);
    Task<IEnumerable<Patient>> GetAllThreads(string userId, CancellationToken ct = default);
    Task<Patient?> GetThreadByIdAsync(string userId, Guid id, CancellationToken ct = default);
}
