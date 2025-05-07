using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data.Repository
{
    public class LivroRepository(BibliotecaContext context) : ILivroRepository
    {
        public void Atualizar(Livro livro)
        {
            context.Livros.Update(livro);
        }

        public async Task<Livro?> Buscar(int id)
        {
            return await context.Livros
                .Include(livro => livro.Autor)
                .Include(livro => livro.Generos)
                .FirstOrDefaultAsync(livro => livro.Id == id);
        }

        public async Task<int> Cadastrar(Livro livro)
        {
            context.Livros.Add(livro);
            await context.SaveChangesAsync();
            return livro.Id;
        }

        public void Deletar(Livro livro)
        {
            context.Livros.Remove(livro);
        }

        public List<Livro> BuscarTodos()
        {
            return [.. context.Livros.Include(l => l.Autor)];
        }
    }
}
