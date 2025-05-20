using Biblioteca.Application.Interface;
using Biblioteca.Application.Models;
using Biblioteca.Application.Models.Genero;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Enums;

namespace Biblioteca.Api.Controller
{
    /// <summary>
    /// Generos
    /// </summary>
    /// <param name="generoService"></param>
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("/v{apiVersion:apiVersion}/generos")]
    public class GeneroController(IGeneroService generoService) : ApiControllerBase
    {
        /// <summary>
        /// Cadastro de um novo gênero
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>O novo gênero criado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     POST /generos
        ///     {
        ///       "nome": "Fantasia"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Gênero cadastrado com sucesso</response>
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
        public async Task<IActionResult> Post([FromBody] CadastrarGeneroViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = await generoService.Cadastrar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Created("/generos", new RetornarCadastroModel(true, "Gênero cadastrado com sucesso", retorno.Data));
        }

        /// <summary>
        /// Buscar um gênero por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>O gênero, caso exista</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     GET /generos/1
        ///
        /// </remarks>
        /// <response code="200">O gênero com id correspondente</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="404">Id não cadastrado</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdministrador,Administrador,Comum")]
        [ProducesResponseType(typeof(BuscarGeneroViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public IActionResult BuscarPorId([FromRoute] int id)
        {
            var genero = generoService.BuscarPorId(id);

            if (genero.IsFailure)
                return FalhaRequisicao(genero.Error);

            return Ok(new BuscarGeneroViewModel(true, "Gênero buscado com sucesso", genero.Data.Id, genero.Data.Nome));
        }
        
        /// <summary>
        /// Editar um gênero previamente cadastrado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns>O id do gênero editado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     PUT /generos
        ///     {
        ///       "nome": "Fantasia"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">O id do gênero que foi atualizado</response>
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
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] AtualizarGeneroViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = await generoService.Atualizar(id, viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RetornarAtualizaModel(true, "Gênero atualizado com sucesso", id));
        }

        /// <summary>
        /// Deletar um gênero previamente cadastrado
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Mensagem de sucesso</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     DELETE /generos/1
        ///
        /// </remarks>
        /// <response code="200">Mensagem de sucesso</response>
        /// <response code="400">Erro no corpo da requisição</response>
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
            var retorno = await generoService.Deletar(id);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RespostaPadraoModel(true, $"Gênero id {retorno.Data} removido com sucesso"));
        }
    }
}
