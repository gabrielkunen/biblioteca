namespace Biblioteca.Application.Models.Funcionario
{
    public class LogarFuncionarioRetornoModel(bool sucesso, string mensagem, string token) : RespostaPadraoModel(sucesso, mensagem)
    {
        public string Token { get; set; } = token;
    }
}
