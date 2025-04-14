namespace Autine.Domain.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : class;
    IPatientRespository Patients { get; }
    IBotRepository Bots { get; }
    Task CommitChangesAsync(CancellationToken ct = default);
}