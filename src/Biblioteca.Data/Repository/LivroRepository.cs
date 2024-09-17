using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data.Repository
{
    public class LivroRepository(BibliotecaContext context) : ILivroRepository
    {
        private readonly BibliotecaContext _context = context;

        public void Atualizar(Livro livro)
        {
            _context.Livros.Update(livro);
        }

        public async Task<Livro?> Buscar(int id)
        {
            return await _context.Livros
                .Include(livro => livro.Autor)
                .Include(livro => livro.Generos)
                .FirstOrDefaultAsync(livro => livro.Id == id);
        }

        public async Task<int> Cadastrar(Livro livro)
        {
            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();
            return livro.Id;
        }

        public void Deletar(Livro livro)
        {
            _context.Livros.Remove(livro);
        }

        public List<Livro> BuscarTodos()
        {
            return [.. _context.Livros.Include(l => l.Autor)];
        }
    }
}
