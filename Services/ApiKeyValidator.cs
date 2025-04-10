using ApiGetaway.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ApiGetaway.Services
{
    public class ApiKeyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ApiKeyDbContext _db;

        public ApiKeyAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ApiKeyDbContext dbContext) : base(options, logger, encoder, clock)
        {
            _db = dbContext;
        }
         protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKeyHeader))
            {
                return AuthenticateResult.Fail("API Key is missing");
            }

            var apiKey = apiKeyHeader.FirstOrDefault();
            if (apiKey == null) return AuthenticateResult.Fail("Invalid API Key");

            var keyRecord = await _db.ApiKeys.FirstOrDefaultAsync(k => k.Key == apiKey);
            if (keyRecord == null)
            {
                return AuthenticateResult.Fail("Unauthorized API Key");
            }

             var claims = new[]
            {
                new Claim(ClaimTypes.Name, keyRecord.Owner),
                new Claim(ClaimTypes.Role, keyRecord.Role), // estándar
                new Claim("role", keyRecord.Role)            // personalizado, usado por Ocelot
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}