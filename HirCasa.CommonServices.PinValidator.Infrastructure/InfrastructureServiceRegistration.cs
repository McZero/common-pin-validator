using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HirCasa.CommonServices.PinValidator.Business.Contracts.Persistence;
using HirCasa.CommonServices.PinValidator.Infrastructure.Persistence;
using HirCasa.CommonServices.PinValidator.Infrastructure.Repositories;

namespace HirCasa.CommonServices.PinValidator.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ContextDb>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ContextDb")));

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
