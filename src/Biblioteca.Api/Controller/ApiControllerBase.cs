using Biblioteca.Application.Models;
using Biblioteca.Application.Models.Result;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controller
{
    /// <summary>
    /// ControllerBase
    /// </summary>
    public abstract class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// Retorno em caso de erro de processamento da requisição
        /// </summary>
        /// <param name="erro"></param>
        /// <returns></returns>
        protected IActionResult FalhaRequisicao(CustomErrorModel erro)
        {
            return StatusCode((int)erro.Codigo, new RespostaPadraoModel(false, erro.Mensagem));
        }
    }
}
