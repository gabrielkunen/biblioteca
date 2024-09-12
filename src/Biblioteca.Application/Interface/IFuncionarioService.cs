using Biblioteca.Application.Models.Funcionario;
using Biblioteca.Application.Models.Result;

namespace Biblioteca.Application.Interface
{
    public interface IFuncionarioService
    {
        Task<CustomResultModel<int>> Cadastrar(string pepper, CadastrarFuncionarioViewModel viewModel);
        CustomResultModel<string> Logar(string pepper, string authToken, LogarFuncionarioViewModel viewModel);
    }
}
