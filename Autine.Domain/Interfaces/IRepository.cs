using System.Linq.Expressions;

namespace Autine.Domain.Interfaces;
public interface IRepository<T>
{
    Task<Guid> Add(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Delete(T entity);
    Task DeleteByIdAsync(CancellationToken ct = default, params object[] keyValues);
    Task<bool> CheckExistAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T> FindByIdAsync(CancellationToken ct = default, params object[] keyValues);
    Task<T> Get(Expression<Func<T, bool>> predicate, string? includes = null, bool tracked = false, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate, string? includes = null, bool tracked = false, CancellationToken ct = default);
}
