namespace HirCasa.CommonServices.PinValidator.Business.Contracts.Settings;

public class ServiceSettings
{
    public int MaxInvalidAttempts { get; set; }
    public int DefaultPinLength { get; set; }
    public int DefaultPinExpirationTime { get; set; }
}
