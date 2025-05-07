using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces.Repository;

namespace Biblioteca.Data.Repository
{
    public class FuncionarioRepository(BibliotecaContext context) : IFuncionarioRepository
    {
        public async Task<int> Cadastrar(Funcionario funcionario)
        {
            context.Funcionarios.Add(funcionario);
            await context.SaveChangesAsync();
            return funcionario.Id;
        }

        public bool JaCadastrado(string email)
        {
            return context.Funcionarios.Any(funcionario => funcionario.Email == email);
        }

        public Funcionario? Buscar(string email)
        {
            return context.Funcionarios.FirstOrDefault(funcionario => funcionario.Email == email);
        }
    }
}
