using System.Text;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;
using Biblioteca.Domain.Interfaces.Reports;

namespace Biblioteca.Data.Reports;

public class RelatorioLivroTxt : IRelatorioLivro
{
    public string NomeArquivo => "relatorio-livros-" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff") + ".txt";
    public ETipoConteudo TipoArquivo => ETipoConteudo.Txt;
    public byte[] GerarRelatorio(List<Livro> livros)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
        
        writer.WriteLine("Id;Titulo;Isbn;Autor;Status");
        foreach (var livro in livros)
            writer.WriteLine($"{livro.Id};{livro.Titulo};{livro.Isbn};{livro.Autor.Nome};{livro.Status.ToString()}");
        
        writer.Flush();
        memoryStream.Position = 0;
        return memoryStream.ToArray();
    }
}