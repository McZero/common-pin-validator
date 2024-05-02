using Microsoft.EntityFrameworkCore;
using HirCasa.CommonServices.PinValidator.Business.Contracts.Persistence;
using HirCasa.CommonServices.PinValidator.Business.Domain.Common;
using HirCasa.CommonServices.PinValidator.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace HirCasa.CommonServices.PinValidator.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseDomainModel
{
    private readonly ContextDb _dbContext;

    public BaseRepository(ContextDb dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        _dbContext.Set<T>().Remove(entity!);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().AnyAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetListAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
    }
}
