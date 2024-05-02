using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HirCasa.CommonServices.PinValidator.Business.Domain;

namespace HirCasa.CommonServices.PinValidator.Infrastructure.Persistence.Configurations;

public class CodigoValidacionConfiguration : BaseConfiguration<CodigoValidacion>
{
    public override void Configure(EntityTypeBuilder<CodigoValidacion> builder)
    {
        base.Configure(builder);

        builder.ToTable("CodigoValidacion");

        builder.Property(x => x.Codigo)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.IdentificadorSolicitante)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.FechaExpiracion)
            .IsRequired();

        builder.Property(x => x.Estado)
            .IsRequired()
            .HasConversion<string>();
    }
}