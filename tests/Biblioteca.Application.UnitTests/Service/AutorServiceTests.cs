using Biblioteca.Application.Models.Autor;
using Biblioteca.Application.Service;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Biblioteca.Domain.Interfaces.Repository;
using FluentAssertions;
using Moq;
using Xunit;

namespace Biblioteca.Application.UnitTests.Service
{
    public class AutorServiceTests
    {
        private readonly AutorService service;
        private readonly Mock<IAutorRepository> autorRepoMock;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;

        public AutorServiceTests() 
        {
            autorRepoMock = new Mock<IAutorRepository>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            service = new AutorService(autorRepoMock.Object, unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Cadastrar_DeveRetornarAutorInvalido()
        {
            // Arrange
            var model = new CadastrarAutorViewModel { Nome = "" };

            // Act
            var retorno = await service.Cadastrar(model);

            // Assert
            retorno.IsFailure.Should().BeTrue();
            retorno.Error.Should().NotBeNull();
        }

        [Fact]
        public async Task Cadastrar_DeveCadastrarComSucesso()
        {
            // Arrange
            var model = new CadastrarAutorViewModel { Nome = "Gabriel", DataNascimento = new DateTime(1995, 12, 20) };

            autorRepoMock.Setup(mock => mock.Cadastrar(It.IsAny<Autor>())).Returns(Task.FromResult(1));

            // Act
            var retorno = await service.Cadastrar(model);

            // Assert
            retorno.IsSuccess.Should().BeTrue();
            retorno.Data.Should().Be(1);
        }

        [Fact]
        public async Task Atualizar_DeveRetornarAutorNaoEncontrado()
        {
            // Arrange
            var model = new AtualizarAutorViewModel { Nome = "William", DataNascimento = new DateTime(1995, 12, 20) };

            autorRepoMock.Setup(mock => mock.Buscar(It.IsAny<int>())).Returns((Autor?)null);

            // Act
            var retorno = await service.Atualizar(1, model);

            // Assert
            retorno.IsFailure.Should().BeTrue();
            retorno.Error.Should().NotBeNull();
        }

        [Fact]
        public async Task Atualizar_DeveRetornarAutorInvalido()
        {
            // Arrange
            var model = new AtualizarAutorViewModel { Nome = "", DataNascimento = new DateTime(1995, 12, 20) };

            autorRepoMock.Setup(mock => mock.Buscar(It.IsAny<int>())).Returns((Autor?)null);

            // Act
            var retorno = await service.Atualizar(1, model);

            // Assert
            retorno.IsFailure.Should().BeTrue();
            retorno.Error.Should().NotBeNull();
        }

        [Fact]
        public async Task Atualizar_DeveAtualizarComSucesso()
        {
            // Arrange
            var model = new AtualizarAutorViewModel { Nome = "William", DataNascimento = new DateTime(1995, 12, 20) };

            autorRepoMock.Setup(mock => mock.Buscar(It.IsAny<int>())).Returns(new Autor("Gabriel", null));

            // Act
            var retorno = await service.Atualizar(1, model);

            // Assert
            retorno.IsSuccess.Should().BeTrue();
        }
    }
}
