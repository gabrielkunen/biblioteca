using Biblioteca.Application.Models.Result;
using Biblioteca.Application.Models.Usuario;

namespace Biblioteca.Application.Interface
{
    public interface IUsuarioService
    {
        Task<CustomResultModel<int>> Cadastrar(CadastrarUsuarioViewModel viewModel);
        CustomResultModel<BuscarUsuarioViewModel> BuscarPorId(int id);
        Task<CustomResultModel<int>> Atualizar(int id, AtualizarUsuarioViewModel viewModel);
        Task<CustomResultModel<int>> Deletar(int id);
    }
}
