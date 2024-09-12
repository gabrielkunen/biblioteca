namespace Biblioteca.Application.Models
{
    public class RespostaPadraoModel(bool sucesso, string mensagem)
    {
        public bool Sucesso { get; private set; } = sucesso;
        public string Mensagem { get; private set; } = mensagem;
    }
}
