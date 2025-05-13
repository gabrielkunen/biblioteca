using System.ComponentModel;

namespace Biblioteca.Domain.Enums;

public enum ETipoConteudo
{
    [Description("application/pdf")]
    Pdf = 0,
    [Description("text/plain")]
    Txt = 1
}