using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;

namespace Biblioteca.Data.Repository
{
    public class AutorRepository(BibliotecaContext context) : IAutorRepository
    {
        public void Atualizar(Autor autor)
        {
            context.Autores.Update(autor);
        }

        public Autor? Buscar(int id)
        {
            return context.Autores.FirstOrDefault(autor => autor.Id == id);
        }

        public async Task<int> Cadastrar(Autor autor)
        {
            context.Autores.Add(autor);
            await context.SaveChangesAsync();
            return autor.Id;
        }

        public void Deletar(Autor autor)
        {
            context.Autores.Remove(autor);
        }

        public bool PossuiLivro(int idAutor)
        {
            return context.Livros.Any(livro => livro.IdAutor == idAutor);
        }
    }
}
