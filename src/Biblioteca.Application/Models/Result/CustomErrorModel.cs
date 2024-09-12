using Biblioteca.Domain.Enums;

namespace Biblioteca.Application.Models.Result
{
    public sealed record CustomErrorModel(ECodigoErro Codigo, string Mensagem)
    {
        public static readonly CustomErrorModel None = new(ECodigoErro.InternaServerError, string.Empty);
    }
}
