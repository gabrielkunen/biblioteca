using Biblioteca.Api.FunctionalTests.Fixture;
using Biblioteca.Application.Models;
using Biblioteca.Application.Models.Autor;
using Biblioteca.Application.Models.Livro;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Biblioteca.Api.FunctionalTests.Controller
{
    public class AutorFunctionalTests : BaseFunctionalTests, IClassFixture<ModelsFixture>
    {
        public ModelsFixture _fixture;
        public AutorFunctionalTests(FunctionalTestWebApplicationFactory factory, ModelsFixture fixture) : base(factory)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisNomeInvalido()
        {
            // Arrange
            var request = new CadastrarAutorViewModel { Nome = "" };

            // Act
            var response = await HttpClient.PostAsJsonAsync("autores", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("O Campo Nome é obrigatório");
        }

        [Fact]
        public async Task Post_DeveRetornarCreated()
        {
            // Arrange

            // Act
            var response = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Autor cadastrado com sucesso");
            responseBody?.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarOk()
        {
            // Arrange
            var responsePost = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Act
            var response = await HttpClient.GetAsync($"autores/{responsePostBody?.Id}");
            var responseBody = await response.Content.ReadFromJsonAsync<BuscarAutorViewModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Autor buscado com sucesso");
            responseBody?.Nome.Should().Be("Gabriel");
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarNotFound()
        {
            // Arrange
            var idAutor = 999999;

            // Act
            var response = await HttpClient.GetAsync($"autores/{idAutor}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Autor id {idAutor} não encontrado");
        }

        [Fact]
        public async Task Delete_DeveRetornarNotFound()
        {
            // Arrange
            var idAutor = 999999;

            // Act
            var response = await HttpClient.DeleteAsync($"autores/{idAutor}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Autor id {idAutor} não encontrado");
        }

        [Fact]
        public async Task Delete_DeveRetornarOk()
        {
            // Arrange
            var responsePost = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Act
            var response = await HttpClient.DeleteAsync($"autores/{responsePostBody?.Id}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be($"Autor id {responsePostBody?.Id} removido com sucesso");
        }

        [Fact]
        public async Task Delete_DeveRetornarBadRequestPoisPossuiLivrosCadastrados()
        {
            // Arrange
            var responsePost = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivroPost = new CadastrarLivroViewModel
            {
                Codigo = "Z100",
                IdAutor = responsePostBody!.Id,
                Isbn = "1234567891234",
                Titulo = "Titulo Livro",
            };
            await HttpClient.PostAsJsonAsync("livros", requestLivroPost);

            // Act
            var response = await HttpClient.DeleteAsync($"autores/{responsePostBody.Id}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Não é possível excluir este autor pois ele possui livros cadastrados.");
        }

        [Fact]
        public async Task Put_DeveRetornarNotFound()
        {
            // Arrange
            var idAutor = 999999;

            // Act
            var response = await HttpClient.PutAsJsonAsync($"autores/{idAutor}", _fixture.AtualizarAutorVmValido);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Autor id {idAutor} não encontrado");
        }

        [Fact]
        public async Task Put_DeveRetornarBadRequestPoisNomeInvalido()
        {
            // Arrange
            var responsePost = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();
            var request = new AtualizarAutorViewModel { Nome = "" };

            // Act
            var response = await HttpClient.PutAsJsonAsync($"autores/{responsePostBody?.Id}", request);
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
            var responsePost = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Act
            var response = await HttpClient.PutAsJsonAsync($"autores/{responsePostBody?.Id}", _fixture.AtualizarAutorVmValido);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarAtualizaModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Autor atualizado com sucesso");
            responseBody?.Id.Should().NotBe(0);
        }
    }
}
