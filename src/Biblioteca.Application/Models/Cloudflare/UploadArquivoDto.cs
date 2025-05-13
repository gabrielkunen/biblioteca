namespace Biblioteca.Application.Models.Cloudflare;

public class UploadArquivoDto(string etag, string nomeArquivo, string tipoArquivo)
{
    public string Etag { get; set; } = etag;
    public string NomeArquivo { get; set; } = nomeArquivo;
    public string TipoArquivo { get; set; } = tipoArquivo;
}