using Biblioteca.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Api.AppConfig;

public static class DbConfig
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
    {
        services.AddDbContext<BibliotecaContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL")));

        return services;
    }
}