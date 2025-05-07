using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;
using Biblioteca.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data.Repository
{
    public class EmprestimoRepository(BibliotecaContext context) : IEmprestimoRepository
    {
        public async Task<int> Cadastrar(Emprestimo emprestimo)
        {
            context.Emprestimos.Add(emprestimo);
            await context.SaveChangesAsync();
            return emprestimo.Id;
        }

        public int QuantidadeEmprestimoAtivo(int idUsuario)
        {
            return context.Emprestimos.Count(e => e.IdUsuario == idUsuario && e.Status == EStatusEmprestimo.Aberto);
        }

        public async Task<Emprestimo?> BuscarEmprestimoAtivo(int idLivro)
        {
            return await context.Emprestimos
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(e => e.IdLivro == idLivro && e.Status == EStatusEmprestimo.Aberto && e.Livro.Status == EStatusLivro.Emprestado);
        }

        public async Task<List<Emprestimo>> Buscar(int take, int page)
        {
            return await context.Emprestimos
                .Include(e => e.Usuario)
                .Include(e => e.Funcionario)
                .AsNoTracking()
                .Skip((page - 1) * take).Take(take).ToListAsync();
        }
    }
}
