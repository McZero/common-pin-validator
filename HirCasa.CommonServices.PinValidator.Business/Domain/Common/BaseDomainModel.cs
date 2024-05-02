namespace HirCasa.CommonServices.PinValidator.Business.Domain.Common;

public abstract class BaseDomainModel
{
    public BaseDomainModel()
    {
        Id = Guid.NewGuid();
        Activo = true;
    }

    public Guid Id { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string UsuarioCreacion { get; set; } = null!;
    public DateTime? FechaModificacion { get; set; }
    public string? UsuarioModificacion { get; set; }
    public bool Activo { get; set; }
}
