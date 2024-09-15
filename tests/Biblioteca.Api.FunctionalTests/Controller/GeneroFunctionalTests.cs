using Biblioteca.Api.FunctionalTests.Fixture;
using Biblioteca.Application.Models;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Biblioteca.Application.Models.Genero;
using FluentAssertions;
using Biblioteca.Application.Models.Livro;

namespace Biblioteca.Api.FunctionalTests.Controller
{
    public class GeneroFunctionalTests : BaseFunctionalTests, IClassFixture<ModelsFixture>
    {
        public ModelsFixture _fixture;
        public GeneroFunctionalTests(FunctionalTestWebApplicationFactory factory, ModelsFixture fixture) : base(factory)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisNomeInvalido()
        {
            // Arrange
            var request = new CadastrarGeneroViewModel { Nome = "" };

            // Act
            var response = await HttpClient.PostAsJsonAsync("generos", request);
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
            var response = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Gênero cadastrado com sucesso");
            responseBody?.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task Put_DeveRetornarNotFound()
        {
            // Arrange
            var idGenero = 999999;

            // Act
            var response = await HttpClient.PutAsJsonAsync($"generos/{idGenero}", _fixture.AtualizarGeneroVmValido);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Gênero id {idGenero} não encontrado");
        }

        [Fact]
        public async Task Put_DeveRetornarBadRequestPoisNomeInvalido()
        {
            // Arrange
            var responsePost = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();
            var request = new AtualizarGeneroViewModel { Nome = "" };

            // Act
            var response = await HttpClient.PutAsJsonAsync($"generos/{responsePostBody?.Id}", request);
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
            var responsePost = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Act
            var response = await HttpClient.PutAsJsonAsync($"generos/{responsePostBody?.Id}", _fixture.AtualizarGeneroVmValido);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarAtualizaModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Gênero atualizado com sucesso");
            responseBody?.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task Delete_DeveRetornarNotFound()
        {
            // Arrange
            var idGenero = 999999;

            // Act
            var response = await HttpClient.DeleteAsync($"generos/{idGenero}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Gênero id {idGenero} não encontrado");
        }

        [Fact]
        public async Task Delete_DeveRetornarOk()
        {
            // Arrange
            var responsePost = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Act
            var response = await HttpClient.DeleteAsync($"generos/{responsePostBody?.Id}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be($"Gênero id {responsePostBody?.Id} removido com sucesso");
        }

        [Fact]
        public async Task Delete_DeveRetornarBadRequestPoisPossuiLivrosCadastrados()
        {
            // Arrange
            var responsePost = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostBody = await responsePost.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivroPost = new CadastrarLivroViewModel
            {
                Codigo = "Z100",
                IdAutor = responsePostBody!.Id,
                Isbn = "1234567891234",
                Titulo = "Titulo Livro",
                Generos = [responsePostGeneroBody!.Id]
            };
            await HttpClient.PostAsJsonAsync("livros", requestLivroPost);

            // Act
            var response = await HttpClient.DeleteAsync($"generos/{responsePostGeneroBody.Id}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Existem livros cadastrados vinculados ao gênero id {responsePostGeneroBody.Id}");
        }
    }
}
