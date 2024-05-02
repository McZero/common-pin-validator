using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HirCasa.CommonServices.PinValidator.Business.Domain.Common;

namespace HirCasa.CommonServices.PinValidator.Infrastructure.Persistence.Configurations;

public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseDomainModel
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.FechaCreacion)
            .IsRequired();

        builder.Property(x => x.UsuarioCreacion)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.UsuarioModificacion)
            .HasMaxLength(100);
    }
}
