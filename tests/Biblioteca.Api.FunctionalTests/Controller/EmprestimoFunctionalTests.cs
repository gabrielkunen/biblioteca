using Biblioteca.Api.FunctionalTests.Fixture;
using Biblioteca.Application.Models;
using Biblioteca.Application.Models.Emprestimo;
using Biblioteca.Application.Models.Funcionario;
using Biblioteca.Application.Models.Livro;
using Biblioteca.Application.Models.Usuario;
using Biblioteca.Domain.Enums;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Biblioteca.Api.FunctionalTests.Controller
{
    public class EmprestimoFunctionalTests : BaseFunctionalTests, IClassFixture<ModelsFixture>
    {
        public ModelsFixture _fixture;
        public EmprestimoFunctionalTests(FunctionalTestWebApplicationFactory factory, ModelsFixture fixture) : base(factory)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisDataInicioInvalida()
        {
            // Arrange
            var request = new CadastrarEmprestimoViewModel { IdLivro = 1, IdUsuario = 1, DataFim = DateTime.Now.AddDays(7)};

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/emprestimos", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be("O Campo DataInicio é obrigatório");
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisLivroInexistente()
        {
            // Arrange
            var request = new CadastrarEmprestimoViewModel { IdLivro = 999999, IdUsuario = 1, DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(7) };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/emprestimos", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Livro id {request.IdLivro} não encontrado");
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisDataFimMenorQueDataInicio()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = 1, DataInicio = DateTime.Now.AddDays(4), DataFim = DateTime.Now.AddDays(2) };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/emprestimos", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"A DataFim precisa ser maior que a DataInicio");
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisUsuarioNaoEncontrado()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = 999999, DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(7) };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/emprestimos", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Usuário id {request.IdUsuario} não encontrado");
        }

        [Fact]
        public async Task Post_DeveRetornarCreated()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Sandra", Email = "sandra@gmail.com", LimiteEmprestimo = 10, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Aluno };

            var responsePostUsuario = await HttpClient.PostAsJsonAsync("v1/usuarios", requestUsuario);
            var responsePostUsuarioBody = await responsePostUsuario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestFuncionario = new CadastrarFuncionarioViewModel { Nome = "Guilherme", Senha = "guilherme.123", Email = "guilherme@gmail.com", Tipo = ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", requestFuncionario);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = responsePostUsuarioBody!.Id, DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(7) };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/emprestimos", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Empréstimo cadastrado com sucesso");
            responseBody?.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisLivroJaEmprestado()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Paula", Email = "paula@gmail.com", LimiteEmprestimo = 10, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Aluno };

            var responsePostUsuario = await HttpClient.PostAsJsonAsync("v1/usuarios", requestUsuario);
            var responsePostUsuarioBody = await responsePostUsuario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestFuncionario = new CadastrarFuncionarioViewModel { Nome = "Guilherme", Senha = "guilherme.123", Email = "guilherme@gmail.com", Tipo = ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", requestFuncionario);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestEmprestimo = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = responsePostUsuarioBody!.Id, DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(7) };

            var responseEmprestimo = await HttpClient.PostAsJsonAsync("v1/emprestimos", requestEmprestimo);
            var responseEmprestimoBody = await responseEmprestimo.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = responsePostUsuarioBody!.Id, DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(7) };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/emprestimos", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Livro id {responsePostLivroBody!.Id} já se encontra emprestado");
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestPoisLimiteEmprestimoUsuarioUltrapassou()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostLivroDois = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroDoisBody = await responsePostLivroDois.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Gislaine", Email = "gislaine@gmail.com", LimiteEmprestimo = 1, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Aluno };

            var responsePostUsuario = await HttpClient.PostAsJsonAsync("v1/usuarios", requestUsuario);
            var responsePostUsuarioBody = await responsePostUsuario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestFuncionario = new CadastrarFuncionarioViewModel { Nome = "Guilherme", Senha = "guilherme.123", Email = "guilherme@gmail.com", Tipo = ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", requestFuncionario);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestEmprestimo = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = responsePostUsuarioBody!.Id, DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(7) };

            var responseEmprestimo = await HttpClient.PostAsJsonAsync("v1/emprestimos", requestEmprestimo);
            var responseEmprestimoBody = await responseEmprestimo.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroDoisBody!.Id, IdUsuario = responsePostUsuarioBody!.Id, DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(7) };

            // Act
            var response = await HttpClient.PostAsJsonAsync("v1/emprestimos", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Usuário id {request.IdUsuario} não pode mais pegar livros emprestados, pois já chegou no limite de {requestUsuario.LimiteEmprestimo} livro(s)");
        }

        [Fact]
        public async Task Devolver_DeveRetornarBadRequestPoisLivroNaoEmprestado()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new DevolverEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id };

            // Act
            var response = await HttpClient.PatchAsJsonAsync("v1/emprestimos/devolver", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Não há empréstimo ativo para o livro id {request.IdLivro}");
        }

        [Fact]
        public async Task Devolver_DeveRetornarBadRequestPoisDataFimExpirada()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Samira", Email = "samira@gmail.com", LimiteEmprestimo = 1, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Aluno };

            var responsePostUsuario = await HttpClient.PostAsJsonAsync("v1/usuarios", requestUsuario);
            var responsePostUsuarioBody = await responsePostUsuario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestFuncionario = new CadastrarFuncionarioViewModel { Nome = "Annie", Senha = "annie.123", Email = "annie@gmail.com", Tipo = ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", requestFuncionario);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestEmprestimo = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = responsePostUsuarioBody!.Id, DataInicio = DateTime.Now.AddDays(-5), DataFim = DateTime.Now.AddDays(-2) };

            var responseEmprestimo = await HttpClient.PostAsJsonAsync("v1/emprestimos", requestEmprestimo);
            var responseEmprestimoBody = await responseEmprestimo.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new DevolverEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id };

            // Act
            var response = await HttpClient.PatchAsJsonAsync("v1/emprestimos/devolver", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"A data de devolução do livro id {request.IdLivro} já expirou, deveria ser devolvido dia {requestEmprestimo.DataFim:dd/MM/yyyy}");
        }

        [Fact]
        public async Task Devolver_DeveRetornarOk()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Rafael", Email = "rafael@gmail.com", LimiteEmprestimo = 1, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Aluno };

            var responsePostUsuario = await HttpClient.PostAsJsonAsync("v1/usuarios", requestUsuario);
            var responsePostUsuarioBody = await responsePostUsuario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestFuncionario = new CadastrarFuncionarioViewModel { Nome = "Joao", Senha = "joao.123", Email = "joao@gmail.com", Tipo = ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", requestFuncionario);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestEmprestimo = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = responsePostUsuarioBody!.Id, DataInicio = DateTime.Now.AddDays(-5), DataFim = DateTime.Now.AddDays(2) };

            var responseEmprestimo = await HttpClient.PostAsJsonAsync("v1/emprestimos", requestEmprestimo);
            var responseEmprestimoBody = await responseEmprestimo.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new DevolverEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id };

            // Act
            var response = await HttpClient.PatchAsJsonAsync("v1/emprestimos/devolver", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Empréstimo devolvido com sucesso");
        }

        [Fact]
        public async Task Renovar_DeveRetornarBadRequestPoisLivroNaoEmprestado()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new RenovarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, QuantidadeDias = 10 };

            // Act
            var response = await HttpClient.PatchAsJsonAsync("v1/emprestimos/renovar", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"Não há empréstimo ativo para o livro id {request.IdLivro}");
        }

        [Fact]
        public async Task Renovar_DeveRetornarBadRequestPoisDataFimExpirada()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Ana", Email = "ana@gmail.com", LimiteEmprestimo = 1, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Aluno };

            var responsePostUsuario = await HttpClient.PostAsJsonAsync("v1/usuarios", requestUsuario);
            var responsePostUsuarioBody = await responsePostUsuario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestFuncionario = new CadastrarFuncionarioViewModel { Nome = "Sofia", Senha = "sofia.123", Email = "sofia@gmail.com", Tipo = ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", requestFuncionario);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestEmprestimo = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = responsePostUsuarioBody!.Id, DataInicio = DateTime.Now.AddDays(-5), DataFim = DateTime.Now.AddDays(-2) };

            var responseEmprestimo = await HttpClient.PostAsJsonAsync("v1/emprestimos", requestEmprestimo);
            var responseEmprestimoBody = await responseEmprestimo.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new RenovarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, QuantidadeDias = 10 };

            // Act
            var response = await HttpClient.PatchAsJsonAsync("v1/emprestimos/renovar", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"A data de devolução do livro id {request.IdLivro} já expirou, deveria ser devolvido dia {requestEmprestimo.DataFim:dd/MM/yyyy}");
        }

        [Fact]
        public async Task Renovar_DeveRetornarBadRequestPoisLivroJaFoiRenovado()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Amanda", Email = "amanda@gmail.com", LimiteEmprestimo = 1, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Aluno };

            var responsePostUsuario = await HttpClient.PostAsJsonAsync("v1/usuarios", requestUsuario);
            var responsePostUsuarioBody = await responsePostUsuario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestFuncionario = new CadastrarFuncionarioViewModel { Nome = "Beatriz", Senha = "beatriz.123", Email = "beatriz@gmail.com", Tipo = ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", requestFuncionario);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestEmprestimo = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = responsePostUsuarioBody!.Id, DataInicio = DateTime.Now.AddDays(-5), DataFim = DateTime.Now.AddDays(1) };

            var responseEmprestimo = await HttpClient.PostAsJsonAsync("v1/emprestimos", requestEmprestimo);
            var responseEmprestimoBody = await responseEmprestimo.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestRenovacao = new RenovarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, QuantidadeDias = 1 };

            var responseRenovacao = await HttpClient.PatchAsJsonAsync("v1/emprestimos/renovar", requestRenovacao);
            var responseRenovacaoBody = await responseRenovacao.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            // Act
            var response = await HttpClient.PatchAsJsonAsync("v1/emprestimos/renovar", requestRenovacao);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody?.Sucesso.Should().BeFalse();
            responseBody?.Mensagem.Should().Be($"O livro id {requestRenovacao.IdLivro} já foi renovado uma vez e não pode ser renovado no momento");
        }

        [Fact]
        public async Task Renovar_DeveRetornarOk()
        {
            // Arrange
            var responsePostAutor = await HttpClient.PostAsJsonAsync("v1/autores", _fixture.CadastrarAutorVmValido);
            var responsePostAutorBody = await responsePostAutor.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var responsePostGenero = await HttpClient.PostAsJsonAsync("v1/generos", _fixture.CadastrarGeneroVmValido);
            var responsePostGeneroBody = await responsePostGenero.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestLivro = new CadastrarLivroViewModel { Codigo = "123", Isbn = "1231231231231", Titulo = "O Pequeno Príncipe", IdAutor = responsePostAutorBody!.Id, Generos = [responsePostGeneroBody!.Id] };

            var responsePostLivro = await HttpClient.PostAsJsonAsync("v1/livros", requestLivro);
            var responsePostLivroBody = await responsePostLivro.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestUsuario = new CadastrarUsuarioViewModel { Nome = "Mariana", Email = "mariana@gmail.com", LimiteEmprestimo = 1, DataNascimento = new DateTime(2000, 12, 10), Tipo = ETipoUsuario.Aluno };

            var responsePostUsuario = await HttpClient.PostAsJsonAsync("v1/usuarios", requestUsuario);
            var responsePostUsuarioBody = await responsePostUsuario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestFuncionario = new CadastrarFuncionarioViewModel { Nome = "José", Senha = "jose.123", Email = "jose@gmail.com", Tipo = ETipoFuncionario.Comum, DataNascimento = new DateTime(2000, 12, 10) };

            var responseFuncionario = await HttpClient.PostAsJsonAsync("v1/funcionarios", requestFuncionario);
            await responseFuncionario.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var requestEmprestimo = new CadastrarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, IdUsuario = responsePostUsuarioBody!.Id, DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(2) };

            var responseEmprestimo = await HttpClient.PostAsJsonAsync("v1/emprestimos", requestEmprestimo);
            var responseEmprestimoBody = await responseEmprestimo.Content.ReadFromJsonAsync<RetornarCadastroModel>();

            var request = new RenovarEmprestimoViewModel { IdLivro = responsePostLivroBody!.Id, QuantidadeDias = 10 };

            // Act
            var response = await HttpClient.PatchAsJsonAsync("v1/emprestimos/renovar", request);
            var responseBody = await response.Content.ReadFromJsonAsync<RespostaPadraoModel>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.Sucesso.Should().BeTrue();
            responseBody?.Mensagem.Should().Be("Empréstimo renovado com sucesso");
        }
    }
}
