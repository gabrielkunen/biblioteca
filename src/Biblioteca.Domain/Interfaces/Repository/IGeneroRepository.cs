using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Interfaces.Repository
{
    public interface IGeneroRepository
    {
        void Atualizar(Genero genero);
        List<Genero> Buscar(List<int> ids);
        Task<Genero?> Buscar(int id);
        Task<int> Cadastrar(Genero genero);
        void Deletar(Genero genero);
        bool PossuiLivro(int idGenero);
    }
}
