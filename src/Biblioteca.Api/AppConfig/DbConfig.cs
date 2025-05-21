using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Api.AppConfig;

/// <summary>
/// Configurações de banco de dados
/// </summary>
public static class DbConfig
{
    /// <summary>
    /// Configurações de banco de dados
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
    {
        services.AddDbContext<BibliotecaContext>(options => options
            .UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL"))
            .UseSeeding((context, _) =>
            {
                if (!context.Set<Genero>().Any())
                {
                    context.Set<Genero>().AddRange(
                        new Genero("Aventura"),
                        new Genero("Fantasia"),
                        new Genero("Ficção científica"),
                        new Genero("Terror"),
                        new Genero("Romance"),
                        new Genero("Suspense"),
                        new Genero("Mistério"),
                        new Genero("Conto"),
                        new Genero("Fábula"),
                        new Genero("Drama"),
                        new Genero("Biografia")
                    );
                }

                if (!context.Set<Autor>().Any())
                {
                    context.Set<Autor>().AddRange(
                        new Autor("Machado de Assis", new DateTime(1839, 06, 21)),
                        new Autor("Érico Veríssimo", new DateTime(1905, 12, 17)),
                        new Autor("Monteiro Lobato", new DateTime(1882, 04, 18)),
                        new Autor("Jorge Amado", new DateTime(1912, 08, 10))
                    );
                }

                if (!context.Set<Livro>().Any())
                {
                    var livro = new Livro("Dom Casmurro", "9780199938117", "DC97", 1);
                    livro.AdicionarGeneros([new Genero("Romance")]);

                    context.Set<Livro>().AddRange(
                        livro,
                        new Livro("Olhai os Lírios do Campo", "9788580860160", "LC97", 2),
                        new Livro("O Picapau Amarelo", "9788551304433", "PA97", 3),
                        new Livro("Capitães da Areia", "9788563397386", "CA97", 4)
                    );
                }

                if (!context.Set<Usuario>().Any())
                {
                    context.Set<Usuario>().AddRange(
                        new Usuario("Raimundo Jorge Danilo Nogueira", "raimundo-nogueira97@gmail.com", 10, null,
                            ETipoUsuario.Aluno),
                        new Usuario("Miguel Geraldo João Castro", "miguel-castro96@inovasom.com", 20,
                            new DateTime(1995, 10, 09), ETipoUsuario.Professor),
                        new Usuario("Silvana Flávia Ribeiro", "silvana-ribeiro89@yahoo.com.br", 15, null,
                            ETipoUsuario.Visitante)
                    );
                }

                if (!context.Set<Funcionario>().Any())
                {
                    context.Set<Funcionario>().AddRange(
                        new Funcionario("SuperAdministrador", Environment.GetEnvironmentVariable("ADMIN_PASSWORD")!,
                            "admin@admin.com.br", new DateTime(1993, 03, 19), ETipoFuncionario.SuperAdministrador)
                    );
                }

                context.SaveChanges();
            }));
        
        return services;
    }
}