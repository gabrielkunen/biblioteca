using Biblioteca.Application.Models.Result;

namespace Biblioteca.Application.Interface
{
    public interface ISenhaService
    {
        CustomResultModel<bool> SenhaValida(string? senha);
        bool SenhaValida(string senhaRequest, string senhaAtual);
        string HashSenha(string senha);
    }
}
