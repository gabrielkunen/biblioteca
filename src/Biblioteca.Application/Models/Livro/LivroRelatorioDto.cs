namespace Biblioteca.Application.Models.Livro;

public class LivroRelatorioDto(string nomeArquivo, string tipoArquivo, byte[] relatorio)
{
    public string NomeArquivo { get; set; } = nomeArquivo;
    public string TipoArquivo { get; set; } = tipoArquivo;
    public byte[] Relatorio { get; set; } = relatorio;
}