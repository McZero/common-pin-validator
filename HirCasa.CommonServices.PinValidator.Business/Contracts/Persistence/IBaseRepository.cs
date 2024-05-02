using System.Linq.Expressions;
using HirCasa.CommonServices.PinValidator.Business.Domain.Common;

namespace HirCasa.CommonServices.PinValidator.Business.Contracts.Persistence;

public interface IBaseRepository<T> where T : BaseDomainModel
{
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<bool> ExistAsync(Expression<Func<T, bool>> predicate);
}
