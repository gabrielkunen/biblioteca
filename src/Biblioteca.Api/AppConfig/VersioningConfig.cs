using Asp.Versioning;

namespace Biblioteca.Api.AppConfig;

/// <summary>
/// Versionamento
/// </summary>
public static class VersioningConfig
{
    /// <summary>
    /// Versionamento
    /// </summary>
    /// <param name="services"></param>
    public static void AddVersioningServices(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }
}