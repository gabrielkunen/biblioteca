namespace Biblioteca.Application.Models.Livro;

public class RetornarGerarRelatorioLivroViewModel(bool sucesso, string mensagem, string etag, string nomeArquivo, string tipoArquivo) : RespostaPadraoModel(sucesso, mensagem)
{
    public string Etag { get; set; } = etag;
    public string NomeArquivo { get; set; } = nomeArquivo;
    public string TipoArquivo { get; set; } = tipoArquivo;
}