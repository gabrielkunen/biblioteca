using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Biblioteca.Api.AppConfig;

/// <summary>
/// Swagger
/// </summary>
public class SwaggerGenConfig : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    /// Swagger
    /// </summary>
    /// <param name="provider"></param>
    public SwaggerGenConfig(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Swagger
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var openApiInfo = new OpenApiInfo
            {
                Title = $"api-biblioteca v{description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "Esta API é um projeto pessoal para simular uma biblioteca, com CRUD de usuários, autores, livros. Empréstimo, devolução, renovação de livros e um relatório do status do acervo.\n\n " +
                              "Como se trata apenas de um ambiente de demonstração, o ambiente é reiniciado a cada 2 horas, junto com a limpeza do banco de dados.\n\n" +
                              "O código fonte pode ser visualizado no [GitHub](https://github.com/gabrielkunen/biblioteca)"
            };
            
            options.SwaggerDoc(description.GroupName, openApiInfo);
        }
    }

    /// <summary>
    /// Swagger
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}