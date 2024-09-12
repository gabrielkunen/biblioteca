using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Interfaces.Repository
{
    public interface IUsuarioRepository
    {
        Task<int> Cadastrar(Usuario usuario);
        void Atualizar(Usuario usuario);
        Usuario? Buscar(int id);
        void Deletar(Usuario usuario);
        bool JaCadastrado(string email);
    }
}
