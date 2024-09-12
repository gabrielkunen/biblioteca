using Biblioteca.Domain.Enums;

namespace Biblioteca.Application.Models.Usuario
{
    public class BuscarUsuarioViewModel(bool sucesso, string mensagem, int id, string nome, string email, string? dataNascimento, ETipoUsuario tipo) : RespostaPadraoModel(sucesso, mensagem)
    {
        public int Id { get; set; } = id;
        public string Nome { get; set; } = nome;
        public string Email { get; set; } = email;
        public string? DataNascimento { get; set; } = dataNascimento;
        public ETipoUsuario Tipo { get; set; } = tipo;
    }
}
