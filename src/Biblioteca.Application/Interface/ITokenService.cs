using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Interface
{
    public interface ITokenService
    {
        string Gerar(string authToken, Funcionario funcionario);
        int BuscarIdFuncionario();
    }
}
