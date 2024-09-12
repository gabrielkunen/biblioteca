using Biblioteca.Application.Models;
using Biblioteca.Application.Models.Result;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controller
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult FalhaRequisicao(CustomErrorModel erro)
        {
            return StatusCode((int)erro.Codigo, new RespostaPadraoModel(false, erro.Mensagem));
        }
    }
}
