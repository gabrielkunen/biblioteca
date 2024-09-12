using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Data.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable(nameof(Usuario));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(e => e.DataNascimento)
                .IsRequired(false)
                .HasColumnType("date");

            builder.Property(e => e.Tipo)
                .IsRequired()
                .HasColumnType("int");
        }
    }
}
