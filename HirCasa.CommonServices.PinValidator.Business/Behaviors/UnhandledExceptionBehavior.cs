using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using HirCasa.CommonServices.PinValidator.Business.Exceptions;

namespace HirCasa.CommonServices.PinValidator.Business.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            // Determinar el tipo de excepción para establecer el nivel de log
            switch (ex)
            {
                case DomainException _:
                case ValidationException _:
                case NotFoundException _:
                case ForbiddenException _:
                    // Logear excepción como warning
                    _logger.LogWarning("Warning: {Message}\nRequest: {Request}\nTrace: {origin}", ex.Message, request, Utility.ParseMessage(ex));
                    break;
                default:
                    // Logear excepción como error
                    _logger.LogError("Unhandled Exception: {Message}\nRequest: {Request}\nTrace: {origin}", ex.Message, request, Utility.ParseMessage(ex));
                    break;
            }

            throw;
        }
    }
}
