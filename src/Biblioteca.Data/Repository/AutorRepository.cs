using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;

namespace Biblioteca.Data.Repository
{
    public class AutorRepository(BibliotecaContext context) : IAutorRepository
    {
        private readonly BibliotecaContext _context = context;

        public void Atualizar(Autor autor)
        {
            _context.Autores.Update(autor);
        }

        public Autor? Buscar(int id)
        {
            return _context.Autores.FirstOrDefault(autor => autor.Id == id);
        }

        public async Task<int> Cadastrar(Autor autor)
        {
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();
            return autor.Id;
        }

        public void Deletar(Autor autor)
        {
            _context.Autores.Remove(autor);
        }

        public bool PossuiLivro(int idAutor)
        {
            return _context.Livros.Any(livro => livro.IdAutor == idAutor);
        }
    }
}
