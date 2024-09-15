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
    [Route("/funcionarios")]
    public class FuncionarioController(IFuncionarioService funcionarioService, IConfiguration configuration) : ApiControllerBase
    {
        private readonly IFuncionarioService _funcionarioService = funcionarioService;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost]
        [Authorize(Roles = "SuperAdministrador")]
        [ProducesResponseType(typeof(RetornarCadastroModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CadastrarFuncionarioViewModel viewModel)
        {
            var retorno = await _funcionarioService.Cadastrar(_configuration["Pepper"]!, viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Created("/autores", new RetornarCadastroModel(true, "Funcionário cadastrado com sucesso", retorno.Data));
        }

        [HttpPost("/funcionarios/logar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LogarFuncionarioRetornoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] LogarFuncionarioViewModel viewModel)
        {
            var retorno = _funcionarioService.Logar(_configuration["Pepper"]!, _configuration["AuthToken"]!, viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new LogarFuncionarioRetornoModel(true, "Funcionário logado com sucesso", retorno.Data));
        }
    }
}
