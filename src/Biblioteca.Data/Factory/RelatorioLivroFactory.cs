using Biblioteca.Application.Interface.Factory;
using Biblioteca.Data.Reports;
using Biblioteca.Domain.Enums;
using Biblioteca.Domain.Interfaces.Reports;

namespace Biblioteca.Data.Factory;

public class RelatorioLivroFactory : IRelatorioLivroFactory
{
    public IRelatorioLivro Gerar(ETipoConteudo tipoConteudo)
    {
        return tipoConteudo switch
        {
            ETipoConteudo.Pdf => new RelatorioLivroPdf(),
            ETipoConteudo.Txt => new RelatorioLivroTxt()
        };
    }
}