using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Biblioteca.Application.Interface;
using Biblioteca.Application.Models;
using Biblioteca.Application.Models.Livro;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Api.Controller
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("/v{apiVersion:apiVersion}/livros")]
    public class LivroController(ILivroService livroService) : ApiControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ProducesResponseType(typeof(RetornarCadastroModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CadastrarLivroViewModel viewModel)
        {
            var retorno = await livroService.Cadastrar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Created("/livros", new RetornarCadastroModel(true, "Livro cadastrado com sucesso", retorno.Data));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdministrador,Administrador,Comum")]
        [ProducesResponseType(typeof(BuscarLivroViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarPorId([FromRoute] int id)
        {
            var livro = await livroService.BuscarPorId(id);

            if (livro.IsFailure)
                return FalhaRequisicao(livro.Error);

            var autor = new BuscarAutorLivroViewModel(livro.Data.Autor.Id, livro.Data.Autor.Nome, livro.Data.Autor.DataNascimento?.ToString("yyyy-MM-dd"));
            var generos = livro.Data.Generos.Select(genero => genero.Nome.ToString()).ToList();

            return Ok(new BuscarLivroViewModel(true, "Livro buscado com sucesso", livro.Data.Id, livro.Data.Titulo, livro.Data.Isbn, livro.Data.Codigo, autor, generos));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ProducesResponseType(typeof(RetornarAtualizaModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] AtualizarLivroViewModel viewModel)
        {
            var retorno = await livroService.Atualizar(id, viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RetornarAtualizaModel(true, "Livro atualizado com sucesso", id));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var retorno = await livroService.Deletar(id);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RespostaPadraoModel(true, $"Livro id {retorno.Data} removido com sucesso"));
        }

        [HttpPost("/v{apiVersion:apiVersion}/livros/relatorio")]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ProducesResponseType(typeof(RetornarGerarRelatorioLivroViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GerarRelatorio([FromBody] GerarRelatorioLivroViewModel viewModel)
        {
            var retorno = await livroService.GerarRelatório(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);
            
            return Ok(new RetornarGerarRelatorioLivroViewModel(true, "Relatório de livros gerado com sucesso.", retorno.Data.Etag, retorno.Data.NomeArquivo, retorno.Data.TipoArquivo));
        }
    }
}
