using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data.Mapping
{
    internal class FuncionarioMapping : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            builder.ToTable(nameof(Funcionario));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Hash)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Salt)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(e => e.DataNascimento)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(e => e.Tipo)
                .IsRequired()
                .HasColumnType("int");
        }
    }
}
