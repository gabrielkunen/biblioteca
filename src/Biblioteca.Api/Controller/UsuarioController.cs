using Biblioteca.Application.Interface;
using Biblioteca.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Biblioteca.Application.Models.Result;
using Biblioteca.Application.Models.Usuario;
using Biblioteca.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Api.Controller
{
    /// <summary>
    /// Usuarios
    /// </summary>
    /// <param name="usuarioService"></param>
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("/v{apiVersion:apiVersion}/usuarios")]
    public class UsuarioController(IUsuarioService usuarioService) : ApiControllerBase
    {
        /// <summary>
        /// Cadastro de um novo usuário
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>O novo usuário criado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     POST /usuarios
        ///     {
        ///       "nome": "William",
        ///       "email": "william@teste.com",
        ///       "limiteEmprestimo": 10,
        ///       "dataNascimento": "2000-05-22",
        ///       "tipo": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Usuário cadastrado com sucesso</response>
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
        public async Task<IActionResult> Post([FromBody] CadastrarUsuarioViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada ou inválida"));
            
            var retorno = await usuarioService.Cadastrar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Created("/usuarios", new RetornarCadastroModel(true, "Usuário cadastrado com sucesso", retorno.Data));
        }

        /// <summary>
        /// Buscar um usuário por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>O usuário, caso exista</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     GET /usuarios/1
        ///
        /// </remarks>
        /// <response code="200">O usuário com id correspondente</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="404">Id não cadastrado</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ProducesResponseType(typeof(BuscarUsuarioViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public IActionResult BuscarPorId([FromRoute] int id)
        {
            var usuario = usuarioService.BuscarPorId(id);

            if (usuario.IsFailure)
                return FalhaRequisicao(usuario.Error);

            return Ok(usuario.Data);
        }

        
        /// <summary>
        /// Editar um usuário previamente cadastrado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns>O id do usuário editado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     PUT /usuarios
        ///     {
        ///       "nome": "William",
        ///       "email": "william@teste.com",
        ///       "limiteEmprestimo": 10,
        ///       "dataNascimento": "2000-05-22",
        ///       "tipo": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="200">O id do usuário que foi atualizado</response>
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
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] AtualizarUsuarioViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada ou inválida"));
            
            var retorno = await usuarioService.Atualizar(id, viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RetornarAtualizaModel(true, "Usuário atualizado com sucesso", id));
        }

        /// <summary>
        /// Deletar um usuário previamente cadastrado
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Mensagem de sucesso</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     DELETE /usuarios/1
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
            var retorno = await usuarioService.Deletar(id);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RespostaPadraoModel(true, $"Usuário id {retorno.Data} removido com sucesso"));
        }
    }
}
