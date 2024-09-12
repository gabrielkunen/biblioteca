using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Data.Mapping
{
    internal class LivroMapping : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.ToTable(nameof(Livro));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Titulo)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Isbn)
                .IsRequired()
                .HasColumnType("varchar(13)");

            builder.Property(e => e.Codigo)
                .IsRequired()
                .HasColumnType("varchar(30)");

            builder.Property(e => e.Status)
                .IsRequired()
                .HasColumnType("int");

            builder.HasOne(e => e.Autor)
                .WithMany(a => a.Livros)
                .HasForeignKey(e => e.IdAutor)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Generos)
                .WithMany(g => g.Livros);
        }
    }
}
