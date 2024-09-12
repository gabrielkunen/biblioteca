using Biblioteca.Application.Models.Emprestimo;
using Biblioteca.Application.Models.Result;

namespace Biblioteca.Application.Interface
{
    public interface IEmprestimoService
    {
        Task<CustomResultModel<int>> Cadastrar(CadastrarEmprestimoViewModel viewModel);
        Task<CustomResultModel<int>> Devolver(DevolverEmprestimoViewModel viewModel);
        Task<CustomResultModel<int>> Renovar(RenovarEmprestimoViewModel viewModel);
    }
}
