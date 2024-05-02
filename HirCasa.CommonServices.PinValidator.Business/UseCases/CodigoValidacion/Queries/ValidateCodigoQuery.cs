using MediatR;

namespace HirCasa.CommonServices.PinValidator.Business.UseCases.CodigoValidacion.Queries;

public class ValidateCodigoQuery : IRequest<bool>
{
    public string Codigo { get; set; } = string.Empty;
    public string IdentificadorSolicitante { get; set; } = string.Empty;
    public string UsuarioModificacion { get; set; } = string.Empty;
}
