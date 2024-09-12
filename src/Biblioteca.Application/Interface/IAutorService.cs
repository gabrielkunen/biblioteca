using Biblioteca.Application.Models.Autor;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Interface
{
    public interface IAutorService
    {
        CustomResultModel<Autor> BuscarPorId(int id);
        Task<CustomResultModel<int>> Cadastrar(CadastrarAutorViewModel autor);
        Task<CustomResultModel<int>> Atualizar(int id, AtualizarAutorViewModel viewModel);
        Task<CustomResultModel<int>> Deletar(int id);
    }
}
