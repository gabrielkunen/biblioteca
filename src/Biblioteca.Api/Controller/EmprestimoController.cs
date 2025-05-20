using Biblioteca.Application.Interface;
using Biblioteca.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Biblioteca.Application.Models.Emprestimo;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Api.Controller
{
    /// <summary>
    /// Emprestimos
    /// </summary>
    /// <param name="emprestimoService"></param>
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("/v{apiVersion:apiVersion}/emprestimos")]
    public class EmprestimoController(IEmprestimoService emprestimoService) : ApiControllerBase
    {
        /// <summary>
        /// Cadastro de um novo empréstimo
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>O novo empréstimo criado</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     POST /emprestimos
        ///     {
        ///       "idLivro": 1,
        ///       "idUsuario": 1,
        ///       "dataInicio": "2025-05-20",
        ///       "dataFim": "2025-05-25"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Empréstimo cadastrado com sucesso</response>
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
        public async Task<IActionResult> Post([FromBody] CadastrarEmprestimoViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = await emprestimoService.Cadastrar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Created("/emprestimos", new RetornarCadastroModel(true, "Empréstimo cadastrado com sucesso", retorno.Data));
        }

        /// <summary>
        /// Realizar a devolução de um livro previamente emprestado
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>Mensagem de sucesso</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     PATCH /emprestimos/devolver
        ///     {
        ///       "idLivro": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Livro devolvido com sucesso</response>
        /// <response code="400">Erro no corpo da requisição</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="500">Erro interno</response>
        [HttpPatch("/v{apiVersion:apiVersion}/emprestimos/devolver")]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Devolver([FromBody] DevolverEmprestimoViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = await emprestimoService.Devolver(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RespostaPadraoModel(true, "Empréstimo devolvido com sucesso"));
        }

        /// <summary>
        /// Realizar a renovação de um livro previamente emprestado
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>Mensagem de sucesso</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     PATCH /emprestimos/renovar
        ///     {
        ///       "idLivro": 1,
        ///       "quantidadeDias" : 5
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Empréstimo renovado com sucesso</response>
        /// <response code="400">Erro no corpo da requisição</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="500">Erro interno</response>
        [HttpPatch("/v{apiVersion:apiVersion}/emprestimos/renovar")]
        [Authorize(Roles = "SuperAdministrador,Administrador")]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Renovar([FromBody] RenovarEmprestimoViewModel viewModel)
        {
            if (viewModel is null)
                return FalhaRequisicao(new CustomErrorModel(ECodigoErro.BadRequest, "Corpo de requisição não enviada"));
            
            var retorno = await emprestimoService.Renovar(viewModel);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new RespostaPadraoModel(true, "Empréstimo renovado com sucesso"));
        }

        /// <summary>
        /// Buscar histórico de empréstimo
        /// </summary>
        /// <param name="take"></param>
        /// <param name="page"></param>
        /// <returns>Os empréstimos paginados</returns>
        /// <remarks>
        /// Request exemplo:
        ///
        ///     GET /emprestimos?take=50&amp;page=1
        ///
        /// </remarks>
        /// <response code="200">O histórico de empréstimo paginado</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Não autorizado</response>
        /// <response code="500">Erro interno</response>
        [HttpGet]
        [Authorize(Roles = "SuperAdministrador,Administrador,Comum")]
        [ProducesResponseType(typeof(BuscarEmprestimoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RespostaPadraoModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] int take = 50, [FromQuery] int page = 1)
        {
            var retorno = await emprestimoService.Buscar(take, page);

            if (retorno.IsFailure)
                return FalhaRequisicao(retorno.Error);

            return Ok(new BuscarEmprestimoViewModel(true, "Empréstimos buscados com sucesso", retorno.Data));
        }
    }
}
