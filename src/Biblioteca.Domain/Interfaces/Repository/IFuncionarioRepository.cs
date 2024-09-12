using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Interfaces.Repository
{
    public interface IFuncionarioRepository
    {
        Task<int> Cadastrar(Funcionario funcionario);
        bool JaCadastrado(string email);
        Funcionario? Buscar(string email);
    }
}
