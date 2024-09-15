using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;
using Biblioteca.Domain.Interfaces;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Enums;
using Biblioteca.Application.Interface;
using Biblioteca.Application.Models.Livro;

namespace Biblioteca.Application.Service
{
    public class LivroService(ILivroRepository livroRepository, IGeneroRepository generoRepository, IUnitOfWork unitOfWork, IAutorRepository autorRepository) : ILivroService
    {
        private readonly ILivroRepository _livroRepository = livroRepository;
        private readonly IGeneroRepository _generoRepository = generoRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAutorRepository _autorRepository = autorRepository;

        public async Task<CustomResultModel<int>> Atualizar(int id, AtualizarLivroViewModel viewModel)
        {
            var livro = await _livroRepository.Buscar(id);

            if (livro == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Livro id {id} não encontrado"));

            livro.Atualizar(viewModel.Titulo, viewModel.Isbn, viewModel.Codigo, viewModel.IdAutor);

            var autor = _autorRepository.Buscar(livro.IdAutor);
            if (autor == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "Id do autor informado não está cadastrado."));

            var generos = _generoRepository.Buscar(viewModel.IdsGeneros);
            if (generos.Count != viewModel.IdsGeneros.Count)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "Ids dos gêneros informados não estão cadastrados."));

            livro.AdicionarGeneros(generos);
            var validacao = livro.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            _livroRepository.Atualizar(livro);
            await _unitOfWork.Commit();
            return CustomResultModel<int>.Success(livro.Id);
        }

        public async Task<CustomResultModel<Livro>> BuscarPorId(int id)
        {
            var livro = await _livroRepository.Buscar(id);

            if (livro == null)
                return CustomResultModel<Livro>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Livro id {id} não encontrado"));

            return CustomResultModel<Livro>.Success(livro);
        }

        public async Task<CustomResultModel<int>> Cadastrar(CadastrarLivroViewModel viewModel)
        {
            var livro = new Livro(viewModel.Titulo, viewModel.Isbn, viewModel.Codigo, viewModel.IdAutor);

            var autor = _autorRepository.Buscar(livro.IdAutor);

            if (autor == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "Id do autor informado não está cadastrado."));

            var generos = _generoRepository.Buscar(viewModel.Generos);

            if (generos.Count != viewModel.Generos.Count)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "Ids dos gêneros informados não estão cadastrados."));

            livro.AdicionarGeneros(generos);

            var validacao = livro.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            var livroId = await _livroRepository.Cadastrar(livro);
            await _unitOfWork.Commit();
            return CustomResultModel<int>.Success(livroId);
        }

        public async Task<CustomResultModel<int>> Deletar(int id)
        {
            var livro = await _livroRepository.Buscar(id);

            if (livro == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Livro id {id} não encontrado"));

            _livroRepository.Deletar(livro);
            await _unitOfWork.Commit();
            return CustomResultModel<int>.Success(livro.Id);
        }
    }
}
