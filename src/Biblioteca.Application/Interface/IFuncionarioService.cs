using Biblioteca.Application.Models.Funcionario;
using Biblioteca.Application.Models.Result;

namespace Biblioteca.Application.Interface
{
    public interface IFuncionarioService
    {
        Task<CustomResultModel<int>> Cadastrar(CadastrarFuncionarioViewModel viewModel);
        CustomResultModel<string> Logar(LogarFuncionarioViewModel viewModel);
    }
}
