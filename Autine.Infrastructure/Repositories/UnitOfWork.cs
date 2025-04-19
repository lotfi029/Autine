using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

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
    public IThreadMemberRepository ThreadMembers 
        => new ThreadMemberRepository(_context);
    public IBotRepository Bots
        => new BotRepository(_context);

    public IBotPatientRepository BotPatients 
        => new BotPatientRepository(_context);

    public IMessageRepository Messages 
        => new MessageRepository(_context);

    public IBotMessageRepository BotMessages 
        => new BotMessageRepository(_context);

    private bool _disposed = false;


    public async Task CommitChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
    }

    public async Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        await transaction.RollbackAsync(cancellationToken);
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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
