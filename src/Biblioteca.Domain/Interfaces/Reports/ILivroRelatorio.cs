using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;

namespace Biblioteca.Domain.Interfaces.Reports;

public interface IRelatorioLivro
{
    string NomeArquivo { get; }
    ETipoConteudo TipoArquivo { get; }
    byte[] GerarRelatorio(List<Livro> livros);
}