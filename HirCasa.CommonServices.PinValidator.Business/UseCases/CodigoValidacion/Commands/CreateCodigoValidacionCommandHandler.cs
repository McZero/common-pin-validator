using HirCasa.CommonServices.PinValidator.Business.Behaviors;
using HirCasa.CommonServices.PinValidator.Business.Contracts.Persistence;
using HirCasa.CommonServices.PinValidator.Business.Contracts.Settings;
using HirCasa.CommonServices.PinValidator.Business.UseCases.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HirCasa.CommonServices.PinValidator.Business.UseCases.CodigoValidacion.Commands;

public class CreateCodigoValidacionCommandHandler : IRequestHandler<CreateCodigoValidacionCommand, CodigoValidacionVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizer _localizer;
    private readonly ServiceSettings _serviceSettings;
    private readonly ILogger<CreateCodigoValidacionCommandHandler> _logger;

    public CreateCodigoValidacionCommandHandler(
        IUnitOfWork unitOfWork,
        ILocalizer localizer,
        IOptions<ServiceSettings> serviceSettings,
        ILogger<CreateCodigoValidacionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
        _serviceSettings = serviceSettings.Value;
        _logger = logger;
    }

    public async Task<CodigoValidacionVm> Handle(CreateCodigoValidacionCommand request, CancellationToken cancellationToken)
    {
        // validar si el UsuarioCreacion e IdentificadorSolicitante existe ya un codigo con estado Generado
        var codigoValidacion = await _unitOfWork.GetRepository<Domain.CodigoValidacion>().GetAsync(
            cv =>
                cv.IdentificadorSolicitante == request.IdentificadorSolicitante
                && cv.UsuarioCreacion == request.UsuarioCreacion
                && cv.Estado == Domain.CodigoValidacion.EstadoCodigoValidacion.Generado);

        // Si existe un codigo con estado Generado, Se valida si el codigo expiro, 
        // si ya expiro se actualiza el codigo y se genera uno nuevo, si no se retorna el codigo existente
        if (codigoValidacion != null)
        {
            if (codigoValidacion.FechaExpiracion < DateTime.UtcNow)
            {
                codigoValidacion.Estado = Domain.CodigoValidacion.EstadoCodigoValidacion.Expirado;
                await _unitOfWork.GetRepository<Domain.CodigoValidacion>().UpdateAsync(codigoValidacion);
            }
            else
            {
                return new CodigoValidacionVm
                {
                    Codigo = codigoValidacion.Codigo,
                    FechaExpiracion = codigoValidacion.FechaExpiracion
                };
            }
        }

        codigoValidacion = new Domain.CodigoValidacion
        {
            Codigo = GenerateRandomCode(request.EsAlfanumerico, request.LongitudCodigo),
            IdentificadorSolicitante = request.IdentificadorSolicitante,
            FechaExpiracion = DateTime.UtcNow.AddSeconds(request.SegundosExpiracion ?? _serviceSettings.DefaultPinExpirationTime),
            Estado = Domain.CodigoValidacion.EstadoCodigoValidacion.Generado,
            Intentos = 0,
            UsuarioCreacion = request.UsuarioCreacion
        };

        await _unitOfWork.GetRepository<Domain.CodigoValidacion>().AddAsync(codigoValidacion);

        _logger.LogInformation(_localizer.GetLoggerMessage("ActionSuccess"),
            request.GetType().Name,
            request.GetType().Name,
            request.IdentificadorSolicitante,
            request.UsuarioCreacion
        );

        return new CodigoValidacionVm
        {
            Codigo = codigoValidacion.Codigo,
            FechaExpiracion = codigoValidacion.FechaExpiracion
        };
    }

    private string GenerateRandomCode(bool esAlfanumerico, int? longitudCodigo)
    {
        if (!longitudCodigo.HasValue)
        {
            longitudCodigo = _serviceSettings.DefaultPinLength;
        }

        var caracteres = esAlfanumerico ? "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789" : "0123456789";
        var random = new Random();
        var codigo = new string(Enumerable.Repeat(caracteres, longitudCodigo.Value).Select(s => s[random.Next(s.Length)]).ToArray());

        return codigo;
    }
}
