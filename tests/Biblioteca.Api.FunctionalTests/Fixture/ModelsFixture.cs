using Biblioteca.Application.Models.Autor;
using Biblioteca.Application.Models.Funcionario;
using Biblioteca.Application.Models.Genero;
using Biblioteca.Application.Models.Usuario;
using Biblioteca.Domain.Enums;

namespace Biblioteca.Api.FunctionalTests.Fixture
{
    public class ModelsFixture
    {
        public CadastrarAutorViewModel CadastrarAutorVmValido;
        public AtualizarAutorViewModel AtualizarAutorVmValido;
        public CadastrarGeneroViewModel CadastrarGeneroVmValido;
        public AtualizarGeneroViewModel AtualizarGeneroVmValido;
        public CadastrarFuncionarioViewModel CadastrarFuncionarioVmValido;
        public CadastrarUsuarioViewModel CadastrarUsuarioVmValido;
        public AtualizarUsuarioViewModel AtualizarUsuarioVmValido;

        public ModelsFixture()
        {
            CadastrarAutorVmValido = new CadastrarAutorViewModel 
            { 
                Nome = "Gabriel", 
                DataNascimento = new DateTime(1995, 10, 12)
            };

            AtualizarAutorVmValido = new AtualizarAutorViewModel
            {
                Nome = "Gabriel",
                DataNascimento = new DateTime(1995, 10, 12)
            };

            CadastrarGeneroVmValido = new CadastrarGeneroViewModel
            {
                Nome = "Fantasia"
            };

            AtualizarGeneroVmValido = new AtualizarGeneroViewModel
            {
                Nome = "Fantasia"
            };
            CadastrarFuncionarioVmValido = new CadastrarFuncionarioViewModel
            {
                Nome = "Pedro",
                DataNascimento = new DateTime(2000, 12, 10),
                Email = "pedro@gmail.com",
                Senha = "Pedro@123",
                Tipo = Domain.Enums.ETipoFuncionario.Comum
            };

            CadastrarUsuarioVmValido = new CadastrarUsuarioViewModel { 
                Nome = "Pedro", 
                Email = "pedro@gmail.com",
                LimiteEmprestimo = 10, 
                DataNascimento = new DateTime(2000, 12, 10), 
                Tipo = ETipoUsuario.Aluno 
            };

            AtualizarUsuarioVmValido = new AtualizarUsuarioViewModel
            {
                Nome = "Pedro",
                Email = "pedro@gmail.com",
                LimiteEmprestimo = 10,
                DataNascimento = new DateTime(2000, 12, 10),
                Tipo = ETipoUsuario.Aluno
            };
        }
    }
}
