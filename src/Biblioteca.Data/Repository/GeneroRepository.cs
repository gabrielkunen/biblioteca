using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data.Repository
{
    public class GeneroRepository(BibliotecaContext context) : IGeneroRepository
    {
        private readonly BibliotecaContext _context = context;

        public void Atualizar(Genero genero)
        {
            _context.Generos.Update(genero);
        }

        public List<Genero> Buscar(List<int> ids)
        {
            return _context.Generos.Where(g => ids.Contains(g.Id)).ToList();
        }

        public async Task<Genero?> Buscar(int id)
        {
            return await _context.Generos.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<int> Cadastrar(Genero genero)
        {
            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();
            return genero.Id;
        }

        public void Deletar(Genero genero)
        {
            _context.Generos.Remove(genero);
        }

        public bool PossuiLivro(int idGenero)
        {
            return _context.Livros.Any(l => l.Generos.Any(g => g.Id == idGenero));
        }
    }
}
