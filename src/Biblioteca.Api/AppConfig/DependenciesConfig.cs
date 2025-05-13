using Biblioteca.Application.Interface;
using Biblioteca.Application.Interface.External;
using Biblioteca.Application.Interface.Factory;
using Biblioteca.Application.Service;
using Biblioteca.Data.External;
using Biblioteca.Data.Factory;
using Biblioteca.Data.Reports;
using Biblioteca.Data.Repository;
using Biblioteca.Domain.Interfaces;
using Biblioteca.Domain.Interfaces.Reports;
using Biblioteca.Domain.Interfaces.Repository;

namespace Biblioteca.Api.AppConfig;

public static class DependenciesConfig
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
        services.AddTransient<IAutorRepository, AutorRepository>();
        services.AddTransient<IGeneroRepository, GeneroRepository>();
        services.AddTransient<ILivroRepository, LivroRepository>();
        services.AddTransient<IUsuarioRepository, UsuarioRepository>();
        services.AddTransient<IFuncionarioRepository, FuncionarioRepository>();
        services.AddTransient<IEmprestimoRepository, EmprestimoRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IAutorService, AutorService>();
        services.AddTransient<ILivroService, LivroService>();
        services.AddTransient<IGeneroService, GeneroService>();
        services.AddTransient<IUsuarioService, UsuarioService>();
        services.AddTransient<IFuncionarioService, FuncionarioService>();
        services.AddTransient<ISenhaService, SenhaService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IEmprestimoService, EmprestimoService>();
        services.AddTransient<IRelatorioLivro, RelatorioLivroPdf>();
        services.AddTransient<IRelatorioLivro, RelatorioLivroTxt>();
        services.AddTransient<IRelatorioLivroFactory, RelatorioLivroFactory>();
        services.AddScoped<ICloudflareR2Client, CloudflareR2Client>();

        return services;
    }
}