using Biblioteca.Application.Models.Genero;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Interface
{
    public interface IGeneroService
    {
        CustomResultModel<Genero> BuscarPorId(int id);
        Task<CustomResultModel<int>> Cadastrar(CadastrarGeneroViewModel viewModel);
        Task<CustomResultModel<int>> Atualizar(int id, AtualizarGeneroViewModel viewModel);
        Task<CustomResultModel<int>> Deletar(int id);
    }
}
