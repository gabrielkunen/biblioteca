namespace Biblioteca.Application.Models
{
    public class RetornarAtualizaModel(bool sucesso, string mensagem, int id) : RespostaPadraoModel(sucesso, mensagem)
    {
        public int Id { get; set; } = id;
    }
}
