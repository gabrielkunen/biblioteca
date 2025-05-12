using Biblioteca.Application.Interface;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Enums;
using System.Text.RegularExpressions;

namespace Biblioteca.Application.Service
{
    public class SenhaService : ISenhaService
    {
        public CustomResultModel<bool> SenhaValida(string? senha)
        {
            if (string.IsNullOrEmpty(senha))
                return CustomResultModel<bool>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Senha não informada"));

            const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            var senhaValida = Regex.IsMatch(senha, pattern);
            return !senhaValida 
                ? CustomResultModel<bool>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "A senha precisa respeitar as seguintes regras: Pelo menos 1 letra maiúscula, 1 letra minúscula, 1 número, 1 caracter especial (@$!%*?&) e ter no mínimo 8 caracteres")) 
                : CustomResultModel<bool>.Success(true);
        }

        public bool SenhaValida(string senhaRequest, string senhaAtual)
        {
            return BCrypt.Net.BCrypt.Verify(senhaRequest, senhaAtual);
        }

        public string HashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }
    }
}
