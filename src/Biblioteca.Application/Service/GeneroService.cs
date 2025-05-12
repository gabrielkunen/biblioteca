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
        public async Task<CustomResultModel<int>> Atualizar(int id, AtualizarGeneroViewModel viewModel)
        {
            var genero = generoRepository.Buscar(id);

            if (genero == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Gênero id {id} não encontrado"));

            genero.Atualizar(viewModel.Nome);
            var validacao = genero.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            generoRepository.Atualizar(genero);
            await unitOfWork.Commit();
            
            return CustomResultModel<int>.Success(genero.Id);
        }

        public CustomResultModel<Genero> BuscarPorId(int id)
        {
            var genero = generoRepository.Buscar(id);

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

            var generoId = await generoRepository.Cadastrar(genero);
            
            return CustomResultModel<int>.Success(generoId);
        }

        public async Task<CustomResultModel<int>> Deletar(int id)
        {
            var genero = generoRepository.Buscar(id);

            if (genero == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Gênero id {id} não encontrado"));

            var possuiLivro = generoRepository.PossuiLivro(id);
            if(possuiLivro)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Existem livros cadastrados vinculados ao gênero id {id}"));

            generoRepository.Deletar(genero);
            await unitOfWork.Commit();
            
            return CustomResultModel<int>.Success(genero.Id);
        }
    }
}
