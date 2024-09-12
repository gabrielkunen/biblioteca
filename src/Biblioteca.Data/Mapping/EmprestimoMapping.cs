using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Data.Mapping
{
    public class EmprestimoMapping : IEntityTypeConfiguration<Emprestimo>
    {
        public void Configure(EntityTypeBuilder<Emprestimo> builder)
        {
            builder.ToTable(nameof(Emprestimo), 
                t => t.HasCheckConstraint("CK_DataFim_MaiorQue_DataInicio", "\"DataFim\" > \"DataInicio\""));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.DataInicio)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(e => e.DataFim)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(e => e.DataDevolucao)
                .IsRequired(false)
                .HasColumnType("date");

            builder.Property(e => e.Renovado)
                .IsRequired();

            builder.HasOne(e => e.Livro)
                .WithMany(a => a.Emprestimos)
                .HasForeignKey(e => e.IdLivro)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Usuario)
                .WithMany(a => a.Emprestimos)
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Funcionario)
                .WithMany(a => a.Emprestimos)
                .HasForeignKey(e => e.IdFuncionario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
