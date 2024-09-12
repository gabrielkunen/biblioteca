namespace Biblioteca.Application.Models.Autor
{
    public class BuscarAutorViewModel(bool sucesso, string mensagem, int id, string nome, DateTime? dataNascimento) : RespostaPadraoModel(sucesso, mensagem)
    {
        public int Id { get; set; } = id;
        public string Nome { get; set; } = nome;
        public DateTime? DataNascimento { get; set; } = dataNascimento;
    }
}
