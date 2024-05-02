using HirCasa.CommonServices.PinValidator.Business.Domain.Common;

namespace HirCasa.CommonServices.PinValidator.Business.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseDomainModel;
}
