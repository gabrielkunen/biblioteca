using Biblioteca.Application.Interface;
using Biblioteca.Application.Models;
using Biblioteca.Application.Models.Autor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Biblioteca.Api.Controller
{
    /// <summary>
    /// Autores
    /// </summary>
    /// <param name="autorService"></param>
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("/v{apiVersion:apiVersion}/autores")]
    public class AutorController(IAutorService autorService) : ApiControllerBase
    {
        
        /// <summary>
        /// Cadastro de um novo autor
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>O novo autor criado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     POST /autores
        ///     {
        ///        "Nome": "John Flanagan",
        ///        "DataNascimento": "1944-05-22",
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Autor cadastrado com sucesso</response>
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
        public async Task<IActionResult> Post([FromBody] CadastrarAutorViewModel viewModel)
        {
            var retorno = await autorService.Cadastrar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Created("/autores", new RetornarCadastroModel(true, "Autor cadastrado com sucesso", retorno.Data));
        }

        /// <summary>
        /// Buscar um autor por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>O autor, caso exista</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     GET /autores/1
        ///
        /// </remarks>
        /// <response code="200">O autor com id correspondente</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="404">Id não cadastrado</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdministrador,Administrador,Comum")]
        [ProducesResponseType(typeof(BuscarAutorViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public IActionResult BuscarPorId([FromRoute] int id)
        {
            var autor = autorService.BuscarPorId(id);

            if (autor.IsFailure)
                return FalhaRequisicao(autor.Error);

            return Ok(new BuscarAutorViewModel(true, "Autor buscado com sucesso", autor.Data.Id, autor.Data.Nome, autor.Data.DataNascimento));
        }

        /// <summary>
        /// Editar um autor previamente cadastrado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns>O autor editado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     PUT /autores
        ///     {
        ///        "Nome": "John Flanagan",
        ///        "DataNascimento": "1944-05-22",
        ///     }
        ///
        /// </remarks>
        /// <response code="200">O id do autor que foi atualizado</response>
        /// <response code="400">Erro na requisição enviada</response>
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
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] AtualizarAutorViewModel viewModel)
        {
            var retorno = await autorService.Atualizar(id, viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RetornarAtualizaModel(true, "Autor atualizado com sucesso", id));
        }

        /// <summary>
        /// Deletar um autor previamente cadastrado
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Mensagem de sucesso</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     DELETE /autores/1
        ///
        /// </remarks>
        /// <response code="200">Mensagem de sucesso</response>
        /// <response code="400">Erro na requisição enviada</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="404">Id não cadastrado</response>
        /// <response code="500">Erro interno</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var retorno = await autorService.Deletar(id);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RespostaPadraoModel(true, $"Autor id {retorno.Data} removido com sucesso"));
        }
    }
}
