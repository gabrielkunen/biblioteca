using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;

namespace Biblioteca.Data.Repository
{
    public class FuncionarioRepository(BibliotecaContext context) : IFuncionarioRepository
    {
        private readonly BibliotecaContext _context = context;
        public async Task<int> Cadastrar(Funcionario funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();
            return funcionario.Id;
        }

        public bool JaCadastrado(string email)
        {
            return _context.Funcionarios.Any(funcionario => funcionario.Email == email);
        }

        public Funcionario? Buscar(string email)
        {
            return _context.Funcionarios.FirstOrDefault(funcionario => funcionario.Email == email);
        }
    }
}
