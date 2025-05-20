using Biblioteca.Application.Interface;
using Biblioteca.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Biblioteca.Application.Models.Funcionario;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Api.Controller
{
    /// <summary>
    /// Funcionarios
    /// </summary>
    /// <param name="funcionarioService"></param>
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("/v{apiVersion:apiVersion}/funcionarios")]
    public class FuncionarioController(IFuncionarioService funcionarioService) : ApiControllerBase
    {
        /// <summary>
        /// Cadastro de um novo funcionário
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>O novo funcionário criado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     POST /funcionarios
        ///     {
        ///       "nome": "Pedro",
        ///       "senha": "Pedro@123",
        ///       "email": "pedro@teste.com",
        ///       "dataNascimento": "1995-01-12",
        ///       "tipo": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Funcionário cadastrado com sucesso</response>
        /// <response code="400">Erro no corpo da requisição</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="500">Erro interno</response>
        [HttpPost]
        [Authorize(Roles = "SuperAdministrador")]
        [ProducesResponseType(typeof(RetornarCadastroModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CadastrarFuncionarioViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = await funcionarioService.Cadastrar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Created("/functionarios", new RetornarCadastroModel(true, "Funcionário cadastrado com sucesso", retorno.Data));
        }

        /// <summary>
        /// Login de um funcionário cadastrado previamente
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>O token para utilizar a api</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     POST /funcionarios/logar
        ///     {
        ///       "email": "pedro@teste.com",
        ///       "senha": "Pedro@123"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Funcionário logado com sucesso</response>
        /// <response code="400">Erro no corpo da requisição</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("/v{apiVersion:apiVersion}/funcionarios/logar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LogarFuncionarioRetornoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] LogarFuncionarioViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = funcionarioService.Logar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new LogarFuncionarioRetornoModel(true, "Funcionário logado com sucesso", retorno.Data));
        }
    }
}
