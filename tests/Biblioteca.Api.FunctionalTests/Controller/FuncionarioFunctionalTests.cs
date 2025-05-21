using Biblioteca.Api.FunctionalTests.Fixture;
using Biblioteca.Application.Models;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Biblioteca.Application.Models.Funcionario;
using FluentAssertions;

namespace Biblioteca.Api.FunctionalTests.Controller
{
    public class FuncionarioFunctionalTests : BaseFunctionalTests, IClassFixture<ModelsFixture>
    {
        public readonly ModelsFixture Fixture;
        public FuncionarioFunctionalTests(FunctionalTestWebApplicationFactory factory, ModelsFixture fixture) : base(factory)
        {
            Fixture = fixture;
        }

        [Fact(Skip = "Método desativado")]
        public async Task Post_DeveRetornarBadRequestPoisSenhaInvalida()
        {
            // Arrange
            var request = new CadastrarFuncionarioViewModel { Nome = "Paulo", Senha = "paulo", Email = "paulo@gmail.com", Tipo = Domain.Enums.ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/funcionarios", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("A senha precisa respeitar as seguintes regras: Pelo menos 1 letra maiúscula, 1 letra minúscula, 1 número, 1 caracter especial (@$!%*?&) e ter no mínimo 8 caracteres");
        }

        [Fact(Skip = "Método desativado")]
        public async Task Post_DeveRetornarBadRequestPoisNomeInvalido()
        {
            // Arrange
            var request = new CadastrarFuncionarioViewModel { Nome = "", Senha = "William@123", Email = "william@gmail.com", Tipo = Domain.Enums.ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/funcionarios", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("O Campo Nome é obrigatório");
        }

        [Fact(Skip = "Método desativado")]
        public async Task Post_DeveRetornarBadRequestPoisEmailJaCadastrado()
        {
            // Arrange
            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", Fixture.CadastrarFuncionarioVmValido);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/funcionarios", Fixture.CadastrarFuncionarioVmValido);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Funcionário com email {Fixture.CadastrarFuncionarioVmValido.Email} já cadastrado.");
        }

        [Fact(Skip = "Método desativado")]
        public async Task Post_DeveRetornarCreated()
        {
            // Arrange
            var request = new CadastrarFuncionarioViewModel { Nome = "Gabriel", Senha = "Gabriel@123", Email = "gabriel@gmail.com", Tipo = Domain.Enums.ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/funcionarios", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Funcionário cadastrado com sucesso");
            responseBody?.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task Logar_DeveRetornarBadRequestEmailSenhaInvalidoPoisEmailNaoCadastrado()
        {
            // Arrange
            var request = new LogarFuncionarioViewModel { Email = "emfoiewrnmuierfn", Senha = "senha" };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/funcionarios/logar", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("Email ou senha inválidos");
        }

        [Fact]
        public async Task Logar_DeveRetornarBadRequestEmailSenhaInvalidoPoisSenhaIncorreta()
        {
            // Arrange
            var requestFuncionario = new CadastrarFuncionarioViewModel { Nome = "Leonardo", Senha = "leonardo.123", Email = "leonardo@gmail.com", Tipo = Domain.Enums.ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", requestFuncionario);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new LogarFuncionarioViewModel { Email = "pedro@gmail.com", Senha = "senha" };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/funcionarios/logar", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("Email ou senha inválidos");
        }

        [Fact(Skip = "Método desativado")]
        public async Task Logar_DeveRetornarOkLogadoComSucesso()
        {
            // Arrange
            var requestFuncionario = new CadastrarFuncionarioViewModel { Nome = "Andre", Senha = "Andre@123", Email = "andre@gmail.com", Tipo = Domain.Enums.ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", requestFuncionario);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new LogarFuncionarioViewModel { Email = "andre@gmail.com", Senha = "Andre@123" };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/funcionarios/logar", request);
            var responseBody = await response.Content.ReadFromJsonAsync<LogarFuncionarioRetornoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Funcionário logado com sucesso");
            responseBody?.Token.Should().NotBeNull();
        }
    }
}
