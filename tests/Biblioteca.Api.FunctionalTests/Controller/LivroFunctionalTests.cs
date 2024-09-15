using Biblioteca.Api.FunctionalTests.Fixture;
using Biblioteca.Application.Models;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Biblioteca.Application.Models.Livro;
using FluentAssertions;

namespace Biblioteca.Api.FunctionalTests.Controller
{
    public class LivroFunctionalTests : BaseFunctionalTests, IClassFixture<ModelsFixture>
    {
        public ModelsFixture _fixture;
        public LivroFunctionalTests(FunctionalTestWebApplicationFactory factory, ModelsFixture fixture) : base(factory)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisAutorInexistente()
        {
            // Arrange
            var request = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = 999999, Generos = [999999] };

            // Act
            var response = await HttpClient.PostAsJsonAsync("livros", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("Id do autor informado não está cadastrado.");
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisGenerosInexistentes()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [999999] };

            // Act
            var response = await HttpClient.PostAsJsonAsync("livros", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("Ids dos gêneros informados não estão cadastrados.");
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisTituloInvalido()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            // Act
            var response = await HttpClient.PostAsJsonAsync("livros", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("O Campo Titulo é obrigatório");
        }

        [Fact]
        public async Task Post_DeveRetornarCreated()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            // Act
            var response = await HttpClient.PostAsJsonAsync("livros", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Livro cadastrado com sucesso");
            responseBody?.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarOk()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("livros", request);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Act
            var response = await HttpClient.GetAsync($"livros/{responsePostLivroBody?.Id}");
            var responseBody = await response.Content.ReadFromJsonAsync<BuscarLivroViewModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Livro buscado com sucesso");
            responseBody?.Titulo.Should().Be("O Pequeno Príncipe");
            responseBody?.Isbn.Should().Be("1231231231231");
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarNotFound()
        {
            // Arrange
            var idLivro = 999999;

            // Act
            var response = await HttpClient.GetAsync($"livros/{idLivro}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Livro id {idLivro} não encontrado");
        }

        [Fact]
        public async Task Put_DeveRetornarNotFound()
        {
            // Arrange
            var request = new AtualizarLivroViewModel { };
            var idLivro = 999999;

            // Act
            var response = await HttpClient.PutAsJsonAsync($"livros/{idLivro}", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Livro id {idLivro} não encontrado");
        }

        [Fact]
        public async Task Put_DeveRetornarBadRequestPoisAutorInexistente()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new AtualizarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = 999999, IdsGeneros = [999999] };

            // Act
            var response = await HttpClient.PutAsJsonAsync($"livros/{responsePostLivroBody!.Id}", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("Id do autor informado não está cadastrado.");
        }

        [Fact]
        public async Task Put_DeveRetornarBadRequestPoisGenerosInexistentes()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new AtualizarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, IdsGeneros = [999999] };

            // Act
            var response = await HttpClient.PutAsJsonAsync($"livros/{responsePostLivroBody!.Id}", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("Ids dos gêneros informados não estão cadastrados.");
        }


        [Fact]
        public async Task Put_DeveRetornarBadRequestPoisTituloInvalido()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new AtualizarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "", IdAutor = responsePostAutorBody!.Id, IdsGeneros = [responsePostGeneroBody!.Id] };

            // Act
            var response = await HttpClient.PutAsJsonAsync($"livros/{responsePostLivroBody!.Id}", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("O Campo Titulo é obrigatório");
        }

        [Fact]
        public async Task Put_DeveRetornarOk()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new AtualizarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe 2", IdAutor = responsePostAutorBody!.Id, IdsGeneros = [responsePostGeneroBody!.Id] };

            // Act
            var response = await HttpClient.PutAsJsonAsync($"livros/{responsePostLivroBody!.Id}", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarAtualizaModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Livro atualizado com sucesso");
            responseBody?.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task Delete_DeveRetornarNotFound()
        {
            // Arrange
            var idLivro = 999999;

            // Act
            var response = await HttpClient.DeleteAsync($"livros/{idLivro}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Livro id {idLivro} não encontrado");
        }

        [Fact]
        public async Task Delete_DeveRetornarOk()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new AtualizarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe 2", IdAutor = responsePostAutorBody!.Id, IdsGeneros = [responsePostGeneroBody!.Id] };

            // Act
            var response = await HttpClient.DeleteAsync($"livros/{responsePostLivroBody?.Id}");
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be($"Livro id {responsePostLivroBody?.Id} removido com sucesso");
        }
    }
}
