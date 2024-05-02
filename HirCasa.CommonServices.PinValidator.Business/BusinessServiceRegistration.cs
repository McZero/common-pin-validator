using System.Globalization;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using HirCasa.CommonServices.PinValidator.Business.Behaviors;
using HirCasa.CommonServices.PinValidator.Business.Contracts.Settings;
using Microsoft.Extensions.Configuration;

namespace HirCasa.CommonServices.PinValidator.Business;

public static class BusinessServiceRegistration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuracion de las opciones del servicio con lo que se encuentra en el archivo appsettings.json
        services.Configure<ServiceSettings>(c => configuration.Bind("ServiceSettings", c));

        // Agregamos el uso de AutoMapper, MediatR y FluentValidation
        services.AddAutoMapper(typeof(BusinessServiceRegistration).Assembly);
        services.AddValidatorsFromAssembly(typeof(BusinessServiceRegistration).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BusinessServiceRegistration).Assembly));

        // Agregamos el comportamiento de las excepciones
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        // Agregamos el comportamiento de las validaciones
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Configuración de la internacionalización para los mensajes.
        services.AddLocalization();

        var supportedCultures = new[]
        {
            new CultureInfo("es-MX"),
            new CultureInfo("en-US")
        };

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("es-MX");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        // Agregamos el servicio de localizacion como singleton
        services.AddSingleton(typeof(ILocalizer), typeof(Localizer));

        return services;
    }
}
