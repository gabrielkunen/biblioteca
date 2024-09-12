using Biblioteca.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Biblioteca.Domain.UnitTests.Entities
{
    public class EmprestimoTests
    {
        [Fact]
        public void DataFimMaiorQueDataInicio_DeveRetornarTrue()
        {
            // Arrange
            var emprestimo = new Emprestimo(1, 1, 1, new DateTime(2024, 10, 10), new DateTime(2024, 10, 17));

            // Act
            var datasValidas = emprestimo.DataFimMaiorQueDataInicio();

            // Assert
            datasValidas.Should().BeTrue();
        }

        [Fact]
        public void DataFimMaiorQueDataInicio_DeveRetornarFalse()
        {
            // Arrange
            var emprestimo = new Emprestimo(1, 1, 1, new DateTime(2024, 10, 17), new DateTime(2024, 10, 10));

            // Act
            var retorno = emprestimo.DataFimMaiorQueDataInicio();

            // Assert
            retorno.Should().BeFalse();
        }

        [Fact]
        public void DevolucaoExpirada_DeveRetornarTrue()
        {
            // Arrange
            var emprestimo = new Emprestimo(1, 1, 1, new DateTime(2024, 10, 17), new DateTime(2023, 10, 10));

            // Act
            var retorno = emprestimo.DevolucaoExpirada();

            // Assert
            retorno.Should().BeTrue();
        }

        [Fact]
        public void DevolucaoExpirada_DeveRetornarFalse()
        {
            // Arrange
            var emprestimo = new Emprestimo(1, 1, 1, new DateTime(2024, 10, 17), new DateTime(9999, 10, 10));

            // Act
            var retorno = emprestimo.DevolucaoExpirada();

            // Assert
            retorno.Should().BeFalse();
        }

        [Fact]
        public void Devolver_DeveAlterarStatusEDataDevolucao()
        {
            // Arrange
            var emprestimo = new Emprestimo(1, 1, 1, new DateTime(2024, 10, 10), new DateTime(2024, 10, 17));

            // Act
            emprestimo.Devolver();

            // Assert
            emprestimo.Status.Should().Be(Enums.EStatusEmprestimo.Encerrado);
            emprestimo.DataDevolucao.Should().NotBeNull();
        }

        [Fact]
        public void MarcarComoRenovacao_DeveMarcarTagRenovadoTrue()
        {
            // Arrange
            var emprestimo = new Emprestimo(1, 1, 1, new DateTime(2024, 10, 10), new DateTime(2024, 10, 17));

            // Act
            emprestimo.MarcarComoRenovacao();

            // Assert
            emprestimo.Renovado.Should().Be(true);
        }

        [Fact]
        public void JaFoiRenovado_DeveRetornarFalse()
        {
            // Arrange
            var emprestimo = new Emprestimo(1, 1, 1, new DateTime(2024, 10, 10), new DateTime(2024, 10, 17));

            // Act
            var retorno = emprestimo.JaFoiRenovado();

            // Assert
            retorno.Should().Be(false);
        }

        [Fact]
        public void Validar_DeveRetornarTrue()
        {
            // Arrange
            var emprestimo = new Emprestimo(1, 1, 1, new DateTime(2024, 10, 10), new DateTime(2024, 10, 17));

            // Act
            var validacao = emprestimo.Validar();

            // Assert
            validacao.IsValid.Should().BeTrue();
            validacao.Errors.Count.Should().Be(0);
        }
    }
}
