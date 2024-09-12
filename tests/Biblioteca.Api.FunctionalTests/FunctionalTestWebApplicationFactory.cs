using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Testcontainers.PostgreSql;
using Xunit;

namespace Biblioteca.Api.FunctionalTests
{
    public class FunctionalTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16.4")
            .WithDatabase("bibliotecadb")
            .WithUsername("biblioteca")
            .WithPassword("postgres")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<BibliotecaContext>));

                services.AddDbContext<BibliotecaContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString()));

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultScheme = "TestScheme";
                }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", options => { });

                var dbContextOptions = new DbContextOptionsBuilder<BibliotecaContext>()
                    .UseNpgsql(_dbContainer.GetConnectionString().ToString())
                    .Options;

                using var context = new BibliotecaContext(dbContextOptions);
                context.Database.Migrate();
            });
        }

        public Task InitializeAsync()
        {
            return _dbContainer.StartAsync();
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
