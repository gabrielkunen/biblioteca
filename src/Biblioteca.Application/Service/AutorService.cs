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
        private readonly IAutorRepository _autorRepository = autorRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public CustomResultModel<Autor> BuscarPorId(int id)
        {
            var autor = _autorRepository.Buscar(id);

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

            var autorId = await _autorRepository.Cadastrar(autor);
            await _unitOfWork.Commit();

            return CustomResultModel<int>.Success(autorId);
        }
        public async Task<CustomResultModel<int>> Atualizar(int id, AtualizarAutorViewModel viewModel)
        {
            var autor = _autorRepository.Buscar(id);

            if (autor == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Autor id {id} não encontrado"));

            autor.Atualizar(viewModel.Nome, viewModel.DataNascimento);
            var validacao = autor.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            _autorRepository.Atualizar(autor);
            await _unitOfWork.Commit();

            return CustomResultModel<int>.Success(autor.Id);
        }

        public async Task<CustomResultModel<int>> Deletar(int id)
        {
            var autor = _autorRepository.Buscar(id);

            if (autor == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Autor id {id} não encontrado"));

            var possuiLivro = _autorRepository.PossuiLivro(id);
            if (possuiLivro)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "Não é possível excluir este autor pois ele possui livros cadastrados."));

            _autorRepository.Deletar(autor);
            await _unitOfWork.Commit();
            return CustomResultModel<int>.Success(autor.Id);
        }
    }
}
