using System.Collections;
using HirCasa.CommonServices.PinValidator.Business.Contracts.Persistence;
using HirCasa.CommonServices.PinValidator.Business.Domain.Common;
using HirCasa.CommonServices.PinValidator.Infrastructure.Persistence;

namespace HirCasa.CommonServices.PinValidator.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable? _repositories;
    private readonly ContextDb _dbContext;

    public ContextDb DbContext => _dbContext;

    public UnitOfWork(ContextDb dbContext)
    {
        _dbContext = dbContext;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseDomainModel
    {
        _repositories ??= new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(BaseRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

            _repositories.Add(type, repositoryInstance);
        }

        return (IBaseRepository<TEntity>)_repositories[type]!;
    }
}
