using Microsoft.Extensions.Localization;
using HirCasa.CommonServices.PinValidator.Business.Resources;

namespace HirCasa.CommonServices.PinValidator.Business.Behaviors;

public interface ILocalizer
{
    string GetValidationMessage(string key);
    string GetLoggerMessage(string key);
    string GetExceptionMessage(string key);
}

public class Localizer : ILocalizer
{
    private readonly IStringLocalizer<ValidationMessages> _validationLocalizer;
    private readonly IStringLocalizer<LoggerMessages> _loggerLocalizer;
    private readonly IStringLocalizer<ExceptionMessages> _exceptionLocalizer;

    public Localizer(
        IStringLocalizer<ValidationMessages> validationLocalizer,
        IStringLocalizer<LoggerMessages> loggerLocalizer,
        IStringLocalizer<ExceptionMessages> exceptionLocalizer)
    {
        _validationLocalizer = validationLocalizer;
        _loggerLocalizer = loggerLocalizer;
        _exceptionLocalizer = exceptionLocalizer;
    }

    public string GetValidationMessage(string key) => _validationLocalizer[key];

    public string GetLoggerMessage(string key) => _loggerLocalizer[key];

    public string GetExceptionMessage(string key) => _exceptionLocalizer[key];
}
