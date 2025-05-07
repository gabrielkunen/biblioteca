using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;

namespace Biblioteca.Data.Repository
{
    public class UsuarioRepository(BibliotecaContext context) : IUsuarioRepository
    {
        public void Atualizar(Usuario usuario)
        {
            context.Usuarios.Update(usuario);
        }

        public Usuario? Buscar(int id)
        {
            return context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
        }

        public async Task<int> Cadastrar(Usuario usuario)
        {
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();
            return usuario.Id;
        }

        public void Deletar(Usuario usuario)
        {
            context.Usuarios.Remove(usuario);
        }

        public bool JaCadastrado(string email)
        {
            return context.Usuarios.Any(usuario => usuario.Email == email);
        }
    }
}
