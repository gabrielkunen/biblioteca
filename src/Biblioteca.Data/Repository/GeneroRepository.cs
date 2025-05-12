using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data.Repository
{
    public class GeneroRepository(BibliotecaContext context) : IGeneroRepository
    {
        public void Atualizar(Genero genero)
        {
            context.Generos.Update(genero);
        }

        public List<Genero> Buscar(List<int> ids)
        {
            return context.Generos.Where(g => ids.Contains(g.Id)).ToList();
        }

        public Genero? Buscar(int id)
        {
            return context.Generos.FirstOrDefault(g => g.Id == id);
        }

        public async Task<int> Cadastrar(Genero genero)
        {
            context.Generos.Add(genero);
            await context.SaveChangesAsync();
            return genero.Id;
        }

        public void Deletar(Genero genero)
        {
            context.Generos.Remove(genero);
        }

        public bool PossuiLivro(int idGenero)
        {
            return context.Livros.Any(l => l.Generos.Any(g => g.Id == idGenero));
        }
    }
}
