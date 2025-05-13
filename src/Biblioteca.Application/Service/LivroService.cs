using System.ComponentModel;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;
using Biblioteca.Domain.Interfaces;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Enums;
using Biblioteca.Application.Interface;
using Biblioteca.Application.Interface.External;
using Biblioteca.Application.Interface.Factory;
using Biblioteca.Application.Models.Cloudflare;
using Biblioteca.Application.Models.Livro;
using Biblioteca.Domain.Helpers;

namespace Biblioteca.Application.Service;

public class LivroService(ILivroRepository livroRepository, IGeneroRepository generoRepository, IUnitOfWork unitOfWork, IAutorRepository autorRepository, 
    ICloudflareR2Client cloudflareR2Client, IRelatorioLivroFactory relatorioLivroFactory) : ILivroService
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

    public async Task<CustomResultModel<UploadArquivoDto>> GerarRelatório(GerarRelatorioLivroViewModel viewModel)
    {
        var validacao = viewModel.Validar();

        if (!validacao.IsValid)
            return CustomResultModel<UploadArquivoDto>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));
            
        var livros = livroRepository.BuscarTodos();
        var relatorioLivro = relatorioLivroFactory.Gerar((ETipoConteudo)viewModel.TipoConteudo!);
        var relatorio = relatorioLivro.GerarRelatorio(livros);
        var contentType = relatorioLivro.TipoArquivo.GetAttributeOfType<DescriptionAttribute>()!.Description;
        
        var retornoUpload = await cloudflareR2Client.UploadArquivo(relatorioLivro.NomeArquivo, contentType, relatorio);
            
        if (retornoUpload.IsFailure)
            return CustomResultModel<UploadArquivoDto>.Failure(retornoUpload.Error with { Mensagem = retornoUpload.Error.Mensagem });

        return CustomResultModel<UploadArquivoDto>.Success(retornoUpload.Data);
    }
}