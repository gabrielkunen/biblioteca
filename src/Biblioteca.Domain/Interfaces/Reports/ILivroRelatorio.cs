using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Interfaces.Reports
{
    public interface ILivroRelatorio
    {
        void GerarRelatorio(List<Livro> livros);
    }
}
