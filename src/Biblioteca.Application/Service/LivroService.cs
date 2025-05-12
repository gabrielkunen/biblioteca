using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;
using Biblioteca.Domain.Interfaces;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Enums;
using Biblioteca.Application.Interface;
using Biblioteca.Application.Models.Livro;
using Biblioteca.Domain.Interfaces.Reports;

namespace Biblioteca.Application.Service
{
    public class LivroService(ILivroRepository livroRepository, IGeneroRepository generoRepository, IUnitOfWork unitOfWork, IAutorRepository autorRepository,
        ILivroRelatorio livroRelatorio) : ILivroService
    {
        public async Task<CustomResultModel<int>> Atualizar(int id, AtualizarLivroViewModel viewModel)
        {
            var livro = await livroRepository.Buscar(id);

            if (livro == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Livro id {id} não encontrado"));

            livro.Atualizar(viewModel.Titulo, viewModel.Isbn, viewModel.Codigo, viewModel.IdAutor);

            var autor = autorRepository.Buscar(livro.IdAutor);
            if (autor == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "Id do autor informado não está cadastrado."));

            var generos = generoRepository.Buscar(viewModel.IdsGeneros);
            if (generos.Count != viewModel.IdsGeneros.Count)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "Ids dos gêneros informados não estão cadastrados."));

            livro.AdicionarGeneros(generos);
            var validacao = livro.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            livroRepository.Atualizar(livro);
            await unitOfWork.Commit();
            return CustomResultModel<int>.Success(livro.Id);
        }

        public async Task<CustomResultModel<Livro>> BuscarPorId(int id)
        {
            var livro = await livroRepository.Buscar(id);

            if (livro == null)
                return CustomResultModel<Livro>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Livro id {id} não encontrado"));

            return CustomResultModel<Livro>.Success(livro);
        }

        public async Task<CustomResultModel<int>> Cadastrar(CadastrarLivroViewModel viewModel)
        {
            var livro = new Livro(viewModel.Titulo, viewModel.Isbn, viewModel.Codigo, viewModel.IdAutor);

            var autor = autorRepository.Buscar(livro.IdAutor);

            if (autor == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "Id do autor informado não está cadastrado."));

            var generos = generoRepository.Buscar(viewModel.Generos);

            if (generos.Count != viewModel.Generos.Count)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "Ids dos gêneros informados não estão cadastrados."));

            livro.AdicionarGeneros(generos);

            var validacao = livro.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            var livroId = await livroRepository.Cadastrar(livro);
            
            return CustomResultModel<int>.Success(livroId);
        }

        public async Task<CustomResultModel<int>> Deletar(int id)
        {
            var livro = await livroRepository.Buscar(id);

            if (livro == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Livro id {id} não encontrado"));

            livroRepository.Deletar(livro);
            await unitOfWork.Commit();
            
            return CustomResultModel<int>.Success(livro.Id);
        }

        public void GerarRelatório()
        {
            var livros = livroRepository.BuscarTodos();

            livroRelatorio.GerarRelatorio(livros);
        }
    }
}
