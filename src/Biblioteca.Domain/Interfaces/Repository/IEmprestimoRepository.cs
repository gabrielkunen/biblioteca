using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Interfaces.Repository
{
    public interface IEmprestimoRepository
    {
        Task<int> Cadastrar(Emprestimo emprestimo);
        int QuantidadeEmprestimoAtivo(int idUsuario);
        Task<Emprestimo?> BuscarEmprestimoAtivo(int idLivro);
    }
}
