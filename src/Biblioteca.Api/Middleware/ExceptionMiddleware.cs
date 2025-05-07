using Biblioteca.Application.Models;
using System.Net;
using System.Text.Json;

namespace Biblioteca.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="next"></param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            RespostaPadraoModel response;

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                response = new RespostaPadraoModel(false, $"Exception: Mensagem: {ex.Message}; InnerException: {ex.InnerException?.Message}; StackTrace: {ex.StackTrace}");
            else
                response = new RespostaPadraoModel(false, "Ocorreu um erro no processamento da sua requisição. Por favor tente novamente mais tarde.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }
    }
}
