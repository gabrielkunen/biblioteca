using Biblioteca.Application.Interface;
using Biblioteca.Application.Models.Autor;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;
using Biblioteca.Domain.Interfaces;
using Biblioteca.Domain.Interfaces.Repository;

namespace Biblioteca.Application.Service
{
    public class AutorService(IAutorRepository autorRepository, IUnitOfWork unitOfWork) : IAutorService
    {
        public CustomResultModel<Autor> BuscarPorId(int id)
        {
            var autor = autorRepository.Buscar(id);

            if (autor == null)
                return CustomResultModel<Autor>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Autor id {id} não encontrado"));

            return CustomResultModel<Autor>.Success(autor);
        }

        public async Task<CustomResultModel<int>> Cadastrar(CadastrarAutorViewModel viewModel)
        {
            var autor = new Autor(viewModel.Nome, viewModel.DataNascimento);
            var validacao = autor.Validar();

            if(!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            var autorId = await autorRepository.Cadastrar(autor);
            await unitOfWork.Commit();

            return CustomResultModel<int>.Success(autorId);
        }
        public async Task<CustomResultModel<int>> Atualizar(int id, AtualizarAutorViewModel viewModel)
        {
            var autor = autorRepository.Buscar(id);

            if (autor == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Autor id {id} não encontrado"));

            autor.Atualizar(viewModel.Nome, viewModel.DataNascimento);
            var validacao = autor.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            autorRepository.Atualizar(autor);
            await unitOfWork.Commit();

            return CustomResultModel<int>.Success(autor.Id);
        }

        public async Task<CustomResultModel<int>> Deletar(int id)
        {
            var autor = autorRepository.Buscar(id);

            if (autor == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Autor id {id} não encontrado"));

            var possuiLivro = autorRepository.PossuiLivro(id);
            if (possuiLivro)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "Não é possível excluir este autor pois ele possui livros cadastrados."));

            autorRepository.Deletar(autor);
            await unitOfWork.Commit();
            return CustomResultModel<int>.Success(autor.Id);
        }
    }
}
