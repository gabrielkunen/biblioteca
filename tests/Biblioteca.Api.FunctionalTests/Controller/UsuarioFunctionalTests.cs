using Biblioteca.Api.FunctionalTests.Fixture;
using Biblioteca.Application.Models;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Biblioteca.Application.Models.Usuario;
using Biblioteca.Domain.Enums;
using FluentAssertions;
using Biblioteca.Application.Models.Autor;

namespace Biblioteca.Api.FunctionalTests.Controller
{
    public class UsuarioFunctionalTests : BaseFunctionalTests, IClassFixture<ModelsFixture>
    {
        public ModelsFixture _fixture;
        public UsuarioFunctionalTests(FunctionalTestWebApplicationFactory factory, ModelsFixture fixture) : base(factory)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisNomeInvalido()
        {
            // Arrange
            var request = new CadastrarUsuarioViewModel { Nome = "", Email = "william@gmail.com", LimiteEmprestimo = 10, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Aluno };

            // Act
            var response = await HttpClient.PostAsJsonAsync("usuarios", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("O Campo Nome é obrigatório");
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisEmailJaCadastrado()
        {
            // Arrange
            var responseUsuario = await HttpClient.PostAsJsonAsync("usuarios", _fixture.CadastrarUsuarioVmValido);
            await responseUsuario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Act
            var response = await HttpClient.PostAsJsonAsync("usuarios", _fixture.CadastrarUsuarioVmValido);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Usuário com email {_fixture.CadastrarUsuarioVmValido.Email} já cadastrado.");
        }

        [Fact]
        public async Task Post_DeveRetornarCreated()
        {
            // Arrange
            var request = new CadastrarUsuarioViewModel { Nome = "Gabriel", Email = "gabriel@gmail.com", LimiteEmprestimo = 10, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Professor };

            // Act
            var response = await HttpClient.PostAsJsonAsync("usuarios", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Usuário cadastrado com sucesso");
            responseBody?.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarNotFound()
        {
            // Arrange
            var idUsuario = 999999;

            // Act
            var response = await HttpClient.GetAsync($"usuarios/{idUsuario}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Usuário id {idUsuario} não encontrado");
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarOk()
        {
            // Arrange
            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Elton", Email = "elton@gmail.com", LimiteEmprestimo = 10, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Visitante };

            var responsePost = await HttpClient.PostAsJsonAsync("usuarios", requestUsuario);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Act
            var response = await HttpClient.GetAsync($"usuarios/{responsePostBody?.Id}");
            var responseBody = await response.Content.ReadFromJsonAsync<BuscarAutorViewModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Usuário buscado com sucesso");
            responseBody?.Nome.Should().Be("Elton");
        }

        [Fact]
        public async Task Delete_DeveRetornarNotFound()
        {
            // Arrange
            var idUsuario = 999999;

            // Act
            var response = await HttpClient.DeleteAsync($"usuarios/{idUsuario}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Usuário id {idUsuario} não encontrado");
        }

        [Fact]
        public async Task Delete_DeveRetornarOk()
        {
            // Arrange
            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Fatima", Email = "fatima@gmail.com", LimiteEmprestimo = 10, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Visitante };

            var responsePost = await HttpClient.PostAsJsonAsync("usuarios", requestUsuario);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Act
            var response = await HttpClient.DeleteAsync($"usuarios/{responsePostBody?.Id}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be($"Usuário id {responsePostBody?.Id} removido com sucesso");
        }

        [Fact]
        public async Task Put_DeveRetornarNotFound()
        {
            // Arrange
            var idUsuario = 999999;

            // Act
            var response = await HttpClient.PutAsJsonAsync($"usuarios/{idUsuario}", _fixture.AtualizarUsuarioVmValido);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Usuário id {idUsuario} não encontrado");
        }

        [Fact]
        public async Task Put_DeveRetornarBadRequestPoisNomeInvalido()
        {
            // Arrange
            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Silvia", Email = "silvia@gmail.com", LimiteEmprestimo = 10, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Visitante };

            var responsePost = await HttpClient.PostAsJsonAsync("usuarios", requestUsuario);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new AtualizarUsuarioViewModel { Nome = "", Email = "silvia@gmail.com", LimiteEmprestimo = 10, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Aluno };

            // Act
            var response = await HttpClient.PutAsJsonAsync($"usuarios/{responsePostBody!.Id}", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("O Campo Nome é obrigatório");
        }

        [Fact]
        public async Task Put_DeveRetornarOk()
        {
            // Arrange
            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Carlos", Email = "carlos@gmail.com", LimiteEmprestimo = 10, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Visitante };

            var responsePost = await HttpClient.PostAsJsonAsync("usuarios", requestUsuario);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new AtualizarUsuarioViewModel { Nome = "Carlos", Email = "carlos@gmail.com", LimiteEmprestimo = 5, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Visitante };

            // Act
            var response = await HttpClient.PutAsJsonAsync($"usuarios/{responsePostBody?.Id}", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarAtualizaModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Usuário atualizado com sucesso");
            responseBody?.Id.Should().NotBe(0);
        }
    }
}
