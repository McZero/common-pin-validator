using HirCasa.CommonServices.PinValidator.Business.Domain.Common;

namespace HirCasa.CommonServices.PinValidator.Business.Domain;

public class CodigoValidacion : BaseDomainModel
{
    public string Codigo { get; set; } = string.Empty;
    public string IdentificadorSolicitante { get; set; } = string.Empty;
    public DateTime FechaExpiracion { get; set; }
    public EstadoCodigoValidacion Estado { get; set; } = EstadoCodigoValidacion.Generado;
    public int Intentos { get; set; }


    // Enumeraciones de estado
    public enum EstadoCodigoValidacion
    {
        Generado,
        Verificado,
        Fallido,
        Expirado
    }
}
