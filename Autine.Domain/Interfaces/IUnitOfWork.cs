namespace Autine.Domain.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : class;
    //IUserRepository<T> GetUserRepository<T>() where T : class;

    IPatientRespository Patients { get; }
    IThreadMemberRepository ThreadMembers { get; }
    IBotRepository Bots { get; }
    IBotPatientRepository BotPatients { get; }
    Task CommitChangesAsync(CancellationToken ct = default);
}