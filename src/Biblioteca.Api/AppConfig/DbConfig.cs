using Biblioteca.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Api.AppConfig;

public static class DbConfig
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BibliotecaContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        return services;
    }
}