using HirCasa.CommonServices.PinValidator.Business.UseCases.ViewModels;
using MediatR;

namespace HirCasa.CommonServices.PinValidator.Business.UseCases.CodigoValidacion.Commands;

public class CreateCodigoValidacionCommand : IRequest<CodigoValidacionVm>
{
    public string IdentificadorSolicitante { get; set; } = string.Empty;
    public int? LongitudCodigo { get; set; }
    public bool EsAlfanumerico { get; set; }
    public int? SegundosExpiracion { get; set; }
    public string UsuarioCreacion { get; set; } = string.Empty;
}
