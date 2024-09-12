using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;

namespace Biblioteca.Data.Repository
{
    public class UsuarioRepository(BibliotecaContext context) : IUsuarioRepository
    {
        private readonly BibliotecaContext _context = context;
        public void Atualizar(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
        }

        public Usuario? Buscar(int id)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
        }

        public async Task<int> Cadastrar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario.Id;
        }

        public void Deletar(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
        }

        public bool JaCadastrado(string email)
        {
            return _context.Usuarios.Any(usuario => usuario.Email == email);
        }
    }
}
