using Biblioteca.Application.Models.Result;

namespace Biblioteca.Application.Interface
{
    public interface ISenhaService
    {
        CustomResultModel<bool> SenhaValida(string? senha);
        string GenerateSalt();
        string ComputeHash(string password, string salt, string pepper, int iteration);
    }
}
