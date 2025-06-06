﻿using Biblioteca.Application.Models.Cloudflare;
using Biblioteca.Application.Models.Livro;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Interface
{
    public interface ILivroService
    {
        Task<CustomResultModel<Livro>> BuscarPorId(int id);
        Task<CustomResultModel<int>> Cadastrar(CadastrarLivroViewModel viewModel);
        Task<CustomResultModel<int>> Atualizar(int id, AtualizarLivroViewModel viewModel);
        Task<CustomResultModel<int>> Deletar(int id);
        Task<CustomResultModel<UploadArquivoDto>> GerarRelatórioCloud(GerarRelatorioLivroViewModel viewModel);
        CustomResultModel<LivroRelatorioDto> GerarRelatório(GerarRelatorioLivroViewModel viewModel);
    }
}
