using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Biblioteca.Api.FunctionalTests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("Id", 1.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, "Test"));
            ci.AddClaim(new Claim(ClaimTypes.Role, "SuperAdministrador"));

            var principal = new ClaimsPrincipal(ci);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
