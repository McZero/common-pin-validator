namespace HirCasa.CommonServices.PinValidator.Business.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }
}
