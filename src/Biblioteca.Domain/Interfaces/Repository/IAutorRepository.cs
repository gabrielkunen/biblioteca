using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Interfaces.Repository
{
    public interface IAutorRepository
    {
        void Atualizar(Autor autor);
        Autor? Buscar(int id);
        Task<int> Cadastrar(Autor autor);
        void Deletar(Autor autor);
        bool PossuiLivro(int idAutor);
    }
}
