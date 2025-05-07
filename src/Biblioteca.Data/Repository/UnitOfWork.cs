using Biblioteca.Data.Context;
using Biblioteca.Domain.Interfaces;

namespace Biblioteca.Data.Repository
{
    public class UnitOfWork(BibliotecaContext context) : IUnitOfWork
    {
        public async Task<int> Commit()
        {
            return await context.SaveChangesAsync();
        }
    }
}
