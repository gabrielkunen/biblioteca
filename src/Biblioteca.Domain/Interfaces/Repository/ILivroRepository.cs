using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Interfaces.Repository
{
    public interface ILivroRepository
    {
        void Atualizar(Livro livro);
        Task<Livro?> Buscar(int id);
        Task<int> Cadastrar(Livro livro);
        void Deletar(Livro livro);
    }
}
