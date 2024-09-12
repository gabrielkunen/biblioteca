using Biblioteca.Domain.Enums;

namespace Biblioteca.Application.Models.Funcionario
{
    public class CadastrarFuncionarioViewModel
    {
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public ETipoFuncionario Tipo { get; set; }
    }
}
