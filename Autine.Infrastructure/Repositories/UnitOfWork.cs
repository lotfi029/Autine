using Autine.Application.Contracts.User;

namespace Autine.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context
        = context ?? throw new ArgumentNullException(nameof(context));

    public IRepository<T> GetRepository<T>() where T : class
        => new Repository<T>(_context);
    //public IUserRepository<T> GetUserRepository<T>() where T : class
    //    => new resp;
    public IPatientRespository Patients
        => new PatientRepository(_context);
    public IBotRepository Bots
        => new BotRepository(_context);

    public IBotPatientRepository BotPatients 
        => new BotPatientRepository(_context);

    private bool _disposed = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task CommitChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
    

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }


}
