namespace Biblioteca.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> Commit();
    }
}
