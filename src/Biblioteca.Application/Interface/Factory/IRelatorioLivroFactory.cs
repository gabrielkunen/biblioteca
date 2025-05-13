using Biblioteca.Domain.Enums;
using Biblioteca.Domain.Interfaces.Reports;

namespace Biblioteca.Application.Interface.Factory;

public interface IRelatorioLivroFactory
{
    IRelatorioLivro Gerar(ETipoConteudo tipoConteudo);
}