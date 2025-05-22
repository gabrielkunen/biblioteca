using Biblioteca.Application.Interface;
using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Biblioteca.Application.Service
{
    public class TokenService(IHttpContextAccessor httpContextAccessor) : ITokenService
    {
        public string Gerar(string authToken, Funcionario funcionario)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(authToken);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GerarClaims(funcionario),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentials,
                Issuer = "gabrielkunen.fun"
            };
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        public int BuscarIdFuncionario()
        {
            var funcionarioLogado = httpContextAccessor.HttpContext!.User;
            return Convert.ToInt32(funcionarioLogado.FindFirst("Id")!.Value);
        }

        private static ClaimsIdentity GerarClaims(Funcionario funcionario)
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("Id", funcionario.Id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, funcionario.Nome));
            ci.AddClaim(new Claim(ClaimTypes.Role, funcionario.Tipo.ToString()));

            return ci;
        }
    }
}
