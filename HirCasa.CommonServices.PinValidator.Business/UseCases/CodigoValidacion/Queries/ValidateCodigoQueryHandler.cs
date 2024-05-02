using HirCasa.CommonServices.PinValidator.Business.Behaviors;
using HirCasa.CommonServices.PinValidator.Business.Contracts.Persistence;
using HirCasa.CommonServices.PinValidator.Business.Contracts.Settings;
using HirCasa.CommonServices.PinValidator.Business.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace HirCasa.CommonServices.PinValidator.Business.UseCases.CodigoValidacion.Queries;

public class ValidateCodigoQueryHandler : IRequestHandler<ValidateCodigoQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ValidateCodigoQueryHandler> _logger;
    private readonly ILocalizer _localizer;
    private readonly ServiceSettings _serviceSettings;

    public ValidateCodigoQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<ValidateCodigoQueryHandler> logger,
        ILocalizer localizer,
        IOptions<ServiceSettings> serviceSettings)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _localizer = localizer;
        _serviceSettings = serviceSettings.Value;
    }

    public async Task<bool> Handle(ValidateCodigoQuery request, CancellationToken cancellationToken)
    {
        var codigoValidacion = await _unitOfWork.GetRepository<Domain.CodigoValidacion>().GetAsync(
                cv =>
                    cv.IdentificadorSolicitante == request.IdentificadorSolicitante
                    && cv.UsuarioCreacion == request.UsuarioModificacion
                    && cv.Estado == Domain.CodigoValidacion.EstadoCodigoValidacion.Generado);

        if (codigoValidacion == null)
        {
            _logger.LogWarning(_localizer.GetLoggerMessage("ActionFail"),
                request.GetType().Name,
                request.GetType().Name,
                request.IdentificadorSolicitante,
                request.UsuarioModificacion
            );

            throw new DomainException(_localizer.GetExceptionMessage("CodeNotFound"));
        }

        codigoValidacion.Intentos++;

        if (codigoValidacion.Intentos > _serviceSettings.MaxInvalidAttempts)
        {
            _logger.LogWarning(_localizer.GetLoggerMessage("ActionFail"),
                request.GetType().Name,
                request.GetType().Name,
                request.IdentificadorSolicitante,
                request.UsuarioModificacion
            );

            codigoValidacion.Estado = Domain.CodigoValidacion.EstadoCodigoValidacion.Fallido;
            await _unitOfWork.GetRepository<Domain.CodigoValidacion>().UpdateAsync(codigoValidacion);

            throw new DomainException(_localizer.GetExceptionMessage("MaxInvalidAttempts"));
        }

        if (codigoValidacion.FechaExpiracion < DateTime.UtcNow)
        {
            _logger.LogWarning(_localizer.GetLoggerMessage("ActionFail"),
                request.GetType().Name,
                request.GetType().Name,
                request.IdentificadorSolicitante,
                request.UsuarioModificacion
            );

            codigoValidacion.Estado = Domain.CodigoValidacion.EstadoCodigoValidacion.Expirado;
            await _unitOfWork.GetRepository<Domain.CodigoValidacion>().UpdateAsync(codigoValidacion);

            throw new DomainException(_localizer.GetExceptionMessage("CodeExpiration"));
        }

        if (codigoValidacion.Codigo != request.Codigo)
        {
            _logger.LogWarning(_localizer.GetLoggerMessage("ActionFail"),
                request.GetType().Name,
                request.GetType().Name,
                request.IdentificadorSolicitante,
                request.UsuarioModificacion
            );

            await _unitOfWork.GetRepository<Domain.CodigoValidacion>().UpdateAsync(codigoValidacion);

            throw new DomainException(_localizer.GetExceptionMessage("InvalidCode"));
        }

        codigoValidacion.Estado = Domain.CodigoValidacion.EstadoCodigoValidacion.Verificado;
        codigoValidacion.UsuarioModificacion = request.UsuarioModificacion;
        await _unitOfWork.GetRepository<Domain.CodigoValidacion>().UpdateAsync(codigoValidacion);

        return true;
    }
}
