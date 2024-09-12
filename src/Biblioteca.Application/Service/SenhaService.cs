using Biblioteca.Application.Interface;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Enums;
using System.Security.Cryptography;
using System.Text;

namespace Biblioteca.Application.Service
{
    public class SenhaService : ISenhaService
    {
        public CustomResultModel<bool> SenhaValida(string? senha)
        {
            if (senha == null || senha == "")
                return CustomResultModel<bool>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Senha não informada"));

            if (senha.Length < 8)
                return CustomResultModel<bool>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Senha precisa ter no mínimo 8 caracteres"));

            if (senha.Length > 60)
                return CustomResultModel<bool>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Senha precisa ter no máximo 60 caracteres"));

            if (!senha.Any(char.IsDigit))
                return CustomResultModel<bool>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Senha precisa ter no mínimo um número"));

            if (!senha.Any(char.IsLetter))
                return CustomResultModel<bool>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Senha precisa ter no mínimo uma letra"));

            return CustomResultModel<bool>.Success(true);
        }

        public string ComputeHash(string password, string salt, string pepper, int iteration)
        {
            if (iteration <= 0) return password;

            var passwordSaltPepper = $"{password}{salt}{pepper}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
            var byteHash = SHA256.HashData(byteValue);
            var hash = Convert.ToBase64String(byteHash);
            return ComputeHash(hash, salt, pepper, iteration - 1);
        }

        public string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            var salt = Convert.ToBase64String(byteSalt);
            return salt;
        }
    }
}
