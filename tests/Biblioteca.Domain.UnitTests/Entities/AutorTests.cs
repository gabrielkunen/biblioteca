using Biblioteca.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Biblioteca.Domain.UnitTests.Entities
{
    public class AutorTests
    {
        [Fact]
        public void Atualizar_DeveAtualizarDados()
        {
            // Arrange
            var autor = new Autor("Gabriel", new DateTime(2000, 06, 12));

            // Act
            autor.Atualizar("William", new DateTime(1999, 04, 11));

            // Assert
            autor.Nome.Should().Be("William");
            autor.DataNascimento.Should().Be(new DateTime(1999, 04, 11));
        }

        [Fact]
        public void Validar_DeveRetornarNomeObrigatorio()
        {
            // Arrange
            var autor = new Autor("", null);

            // Act
            var validacao = autor.Validar();

            // Assert
            validacao.IsValid.Should().BeFalse();
            validacao.Errors.Count.Should().Be(1);
        }

        [Fact]
        public void Validar_DeveRetornarValido()
        {
            // Arrange
            var autor = new Autor("Gabriel", null);

            // Act
            var validacao = autor.Validar();

            // Assert
            validacao.IsValid.Should().BeTrue();
            validacao.Errors.Count.Should().Be(0);
        }
    }
}
