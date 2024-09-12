using Biblioteca.Application.Interface;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Biblioteca.Domain.Interfaces.Repository;
using Biblioteca.Domain.Enums;
using Biblioteca.Application.Models.Genero;

namespace Biblioteca.Application.Service
{
    public class GeneroService(IGeneroRepository generoRepository, IUnitOfWork unitOfWork) : IGeneroService
    {
        private readonly IGeneroRepository _generoRepository = generoRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CustomResultModel<int>> Atualizar(int id, AtualizarGeneroViewModel viewModel)
        {
            var genero = await _generoRepository.Buscar(id);

            if (genero == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Gênero id {id} não encontrado"));

            genero.Atualizar(viewModel.Nome);
            var validacao = genero.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            _generoRepository.Atualizar(genero);
            await _unitOfWork.Commit();
            return CustomResultModel<int>.Success(genero.Id);
        }

        public async Task<CustomResultModel<Genero>> BuscarPorId(int id)
        {
            var genero = await _generoRepository.Buscar(id);

            if (genero == null)
                return CustomResultModel<Genero>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Gênero id {id} não encontrado"));

            return CustomResultModel<Genero>.Success(genero);
        }

        public async Task<CustomResultModel<int>> Cadastrar(CadastrarGeneroViewModel viewModel)
        {
            var genero = new Genero(viewModel.Nome);

            var validacao = genero.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            var generoId = await _generoRepository.Cadastrar(genero);
            await _unitOfWork.Commit();
            return CustomResultModel<int>.Success(generoId);
        }

        public async Task<CustomResultModel<int>> Deletar(int id)
        {
            var genero = await _generoRepository.Buscar(id);

            if (genero == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Gênero id {id} não encontrado"));

            var possuiLivro = _generoRepository.PossuiLivro(id);
            if(possuiLivro)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Existem livros cadastrados vinculados ao gênero id {id}"));

            _generoRepository.Deletar(genero);
            await _unitOfWork.Commit();
            return CustomResultModel<int>.Success(genero.Id);
        }
    }
}
