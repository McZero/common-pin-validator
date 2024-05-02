using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

using Serilog;

using HirCasa.CommonServices.PinValidator.API.Middlewares;
using HirCasa.CommonServices.PinValidator.Business;
using HirCasa.CommonServices.PinValidator.Infrastructure;
using HirCasa.CommonServices.PinValidator.Infrastructure.Persistence;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Inicialización de Serilog 
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    // Configuración de Swagger y API Versioning
    #region Common Setup API
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    builder.Host.UseSerilog();
    builder.Services.AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions = true;
        opt.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("x-api-version"),
            new MediaTypeApiVersionReader("x-api-version"));
    });
    builder.Services.AddVersionedApiExplorer(setup =>
    {
        setup.GroupNameFormat = "'v'VVV";
        setup.SubstituteApiVersionInUrl = true;
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath, true);
        c.OperationFilter<AcceptLanguageHeaderParameter>();
    });
    builder.Services.AddHttpContextAccessor();
    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
    builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
    #endregion

    // Registro de servicios de las capas de infraestructura y negocio
    #region Services Registration

    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddBusinessServices(builder.Configuration);

    #endregion

    // Ejecución del pipeline de la aplicación
    #region Application Pipeline

    var app = builder.Build();
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    //}
    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }
    else
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseRequestLocalization();
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    app.MapControllers();

    // Configuración para correr migraciones al levantar el API
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ContextDb>();
        await context.Database.MigrateAsync();
    }

    app.Run();

    #endregion
}
catch (Exception ex)
{
    Log.Fatal(ex, "Microservice terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}