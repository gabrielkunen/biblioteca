using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Biblioteca.Application.Interface;
using Biblioteca.Application.Models;
using Biblioteca.Application.Models.Livro;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Api.Controller
{
    /// <summary>
    /// Livros
    /// </summary>
    /// <param name="livroService"></param>
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("/v{apiVersion:apiVersion}/livros")]
    public class LivroController(ILivroService livroService) : ApiControllerBase
    {
        /// <summary>
        /// Cadastro de um novo livro
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>O novo livro criado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     POST /livros
        ///     {
        ///       "titulo": "The Ruins of Gorlan: Book One: 01",
        ///       "isbn": "0142406635",
        ///       "codigo": "ABC163",
        ///       "idAutor": 1,
        ///       "generos": [1, 2]
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Livro cadastrado com sucesso</response>
        /// <response code="400">Erro no corpo da requisição</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="500">Erro interno</response>
        [HttpPost]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ProducesResponseType(typeof(RetornarCadastroModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CadastrarLivroViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = await livroService.Cadastrar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Created("/livros", new RetornarCadastroModel(true, "Livro cadastrado com sucesso", retorno.Data));
        }

        /// <summary>
        /// Buscar um livro por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>O livro, caso exista</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     GET /livros/1
        ///
        /// </remarks>
        /// <response code="200">O livro com id correspondente</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="404">Id não cadastrado</response>
        /// <response code="500">Erro interno</response>
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

        /// <summary>
        /// Editar um livro previamente cadastrado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns>O id do livro editado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     PUT /livros
        ///     {
        ///       "titulo": "The Ruins of Gorlan: Book One: 01",
        ///       "isbn": "0142406635",
        ///       "codigo": "ABC163",
        ///       "idAutor": 1,
        ///       "idsGeneros": [1, 2]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">O id do livro que foi atualizado</response>
        /// <response code="400">Erro no corpo da requisição</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="404">Id não cadastrado</response>
        /// <response code="500">Erro interno</response>
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
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = await livroService.Atualizar(id, viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RetornarAtualizaModel(true, "Livro atualizado com sucesso", id));
        }

        /// <summary>
        /// Deletar um livro previamente cadastrado
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Mensagem de sucesso</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     DELETE /livros/1
        ///
        /// </remarks>
        /// <response code="200">Mensagem de sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="404">Id não cadastrado</response>
        /// <response code="500">Erro interno</response>
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

        /// <summary>
        /// Gerar um relatório dos livros do acervo da biblioteca na cloudflare R2
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>Mensagem de sucesso</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     POST /livros/relatorio-cloud/1
        ///     {
        ///       "tipoConteudo": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Mensagem de sucesso</response>
        /// <response code="400">Erro no corpo da requisição</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("/v{apiVersion:apiVersion}/livros/relatorio-cloud")]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(typeof(RetornarGerarRelatorioLivroViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GerarRelatorioCloud([FromBody] GerarRelatorioLivroViewModel viewModel)
        {
            return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Método desativado neste ambiente"));
            
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = await livroService.GerarRelatórioCloud(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);
            
            return Ok(new RetornarGerarRelatorioLivroViewModel(true, "Relatório de livros gerado com sucesso.", retorno.Data.Etag, retorno.Data.NomeArquivo, retorno.Data.TipoArquivo));
        }
        
        /// <summary>
        /// Gerar um relatório dos livros do acervo da biblioteca
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>Relatório gerado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     POST /livros/relatorio/1
        ///     {
        ///       "tipoConteudo": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Relatório gerado</response>
        /// <response code="400">Erro no corpo da requisição</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("/v{apiVersion:apiVersion}/livros/relatorio")]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ProducesResponseType(typeof(File), StatusCodes.Status200OK, "application/pdf")]
        [ProducesResponseType(typeof(File), StatusCodes.Status200OK, "text/plain")]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public IActionResult GerarRelatorio([FromBody] GerarRelatorioLivroViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = livroService.GerarRelatório(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);
            
            return File(retorno.Data.Relatorio, retorno.Data.TipoArquivo, retorno.Data.NomeArquivo);        }
    }
}
