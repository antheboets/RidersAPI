using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Riders.Infrastructure
{
    internal class CustomAuthHandler : AuthenticationHandler<CustomAuthOptions>
    {
        private const string AuthorizationHeaderName = "x-api-key";
        private const string BasicSchemeName = "Basic";
        public CustomAuthHandler(IOptionsMonitor<CustomAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            // store custom services here...
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // build the claims and put them in "Context"; you need to import the Microsoft.AspNetCore.Authentication package
            if (!Request.Headers.ContainsKey(AuthorizationHeaderName))
            {
                //Authorization header not in request

                return AuthenticateResult.Fail("{message: 'ga weg'}");
            }
            if (Request.Headers[AuthorizationHeaderName].Equals("J8vsMkN6NJYrzRc7vutfmpZbGCPCL7bZv5zvhWTpcA7qN56egC5D7CG6Uzx4T7cb"))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, "") };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            return AuthenticateResult.Fail("{message: 'ga weg'}");
        }
    }
}
