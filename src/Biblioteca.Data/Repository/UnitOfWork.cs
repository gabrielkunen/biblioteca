using Biblioteca.Data.Context;
using Biblioteca.Domain.Interfaces;

namespace Biblioteca.Data.Repository
{
    public class UnitOfWork(BibliotecaContext context) : IUnitOfWork
    {
        private readonly BibliotecaContext _context = context;
        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
