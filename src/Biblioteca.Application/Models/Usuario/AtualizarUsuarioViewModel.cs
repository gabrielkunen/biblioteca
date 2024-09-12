using Biblioteca.Domain.Enums;

namespace Biblioteca.Application.Models.Usuario
{
    public class AtualizarUsuarioViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public int LimiteEmprestimo { get; set; }
        public DateTime? DataNascimento { get; set; }
        public ETipoUsuario Tipo { get; set; }
    }
}
