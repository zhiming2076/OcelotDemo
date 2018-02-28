using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace APIGatewayDemo.Common.Middlewares
{
    public class WapAuthenticationHandler : IAuthenticationHandler
    {
        public AuthenticationScheme Scheme { get; private set; }
        protected HttpContext Context { get; private set; }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            this.Scheme = scheme;
            this.Context = context;
            return Task.CompletedTask;
        }

        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            string token = Context.Request.Headers["Token"];
            if (string.IsNullOrEmpty(token))
            {
                AuthenticateResult.Fail("Unauthorized");
                Context.Response.StatusCode = 401; //Unauthorized

                return AuthenticateResult.NoResult();
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "Shanzm"));
            claims.Add(new Claim(ClaimTypes.Role, "Users"));
            var identity = new ClaimsIdentity(claims, "WapIdentity");

            Context.User = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(Context.User, new AuthenticationProperties(), "WapScheme");
            return AuthenticateResult.Success(ticket);
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/login");
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }
    }
}
