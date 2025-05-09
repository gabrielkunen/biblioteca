using Biblioteca.Application.Interface;
using Biblioteca.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Biblioteca.Application.Models.Funcionario;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Api.Controller
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("/v{apiVersion:apiVersion}/funcionarios")]
    public class FuncionarioController(IFuncionarioService funcionarioService, IConfiguration configuration) : ApiControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "SuperAdministrador")]
        [ProducesResponseType(typeof(RetornarCadastroModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CadastrarFuncionarioViewModel viewModel)
        {
            var retorno = await funcionarioService.Cadastrar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Created("/autores", new RetornarCadastroModel(true, "Funcionário cadastrado com sucesso", retorno.Data));
        }

        [HttpPost("/v{apiVersion:apiVersion}/funcionarios/logar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LogarFuncionarioRetornoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] LogarFuncionarioViewModel viewModel)
        {
            var retorno = funcionarioService.Logar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new LogarFuncionarioRetornoModel(true, "Funcionário logado com sucesso", retorno.Data));
        }
    }
}
