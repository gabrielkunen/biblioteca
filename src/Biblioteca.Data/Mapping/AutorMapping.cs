using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Data.Mapping
{
    public class AutorMapping : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> builder)
        {
            builder.ToTable(nameof(Autor));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.DataNascimento)
                .IsRequired(false)
                .HasColumnType("date");

            builder.HasMany(e => e.Livros)
                .WithOne(l => l.Autor)
                .HasForeignKey(l => l.IdAutor)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
