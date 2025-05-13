using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Interfaces.Reports
{
    public interface ILivroRelatorio
    {
        byte[] GerarRelatorio(List<Livro> livros);
    }
}
