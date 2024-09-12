namespace Biblioteca.Application.Models
{
    public class RetornarCadastroModel(bool sucesso, string mensagem, int id) : RespostaPadraoModel(sucesso, mensagem)
    {
        public int Id { get; set; } = id;
    }
}
