using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Ocelot.Configuration;
using Ocelot.Configuration.Provider;
using Ocelot.Configuration.Repository;
using Ocelot.DownstreamRouteFinder;
using Ocelot.DownstreamRouteFinder.Finder;
using Ocelot.DownstreamRouteFinder.UrlMatcher;
using Ocelot.Infrastructure.RequestData;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace APIGatewayDemo.Common.Middlewares
{
    /// <summary>
    /// 【Middleware】使用敏捷平台认证方式
    /// </summary>
    public class WapAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOcelotConfigurationProvider _ocelotConfigurationProvider;
        private readonly IUrlPathToUrlTemplateMatcher _urlMatcher;
        private readonly IPlaceholderNameAndValueFinder _urlPathPlaceholderNameAndValueFinder;

        public WapAuthenticationMiddleware(RequestDelegate next, IOcelotConfigurationProvider ocelotConfigurationProvider, IUrlPathToUrlTemplateMatcher urlMatcher, IPlaceholderNameAndValueFinder urlPathPlaceholderNameAndValueFinder)
        {
            _ocelotConfigurationProvider = ocelotConfigurationProvider;
            _urlMatcher = urlMatcher;
            _urlPathPlaceholderNameAndValueFinder = urlPathPlaceholderNameAndValueFinder;

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            DownstreamRouteFinder downstreamRouteFinder = new DownstreamRouteFinder(_urlMatcher, _urlPathPlaceholderNameAndValueFinder);
            DownstreamRoute route = downstreamRouteFinder.FindDownstreamRoute(context.Request.Path.ToString(), context.Request.Method, _ocelotConfigurationProvider.Get().Result.Data, context.Request.Headers["Host"]).Data;

            if (route.ReRoute.IsAuthenticated)
            {
                string token = context.Request.Headers["Token"];
                if (string.IsNullOrEmpty(token))
                {
                    AuthenticateResult.Fail("Unauthorized");
                    context.Response.StatusCode = 401; //Unauthorized

                    return;
                }

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, "Shanzm"));
                claims.Add(new Claim(ClaimTypes.Role, "Users"));
                var identity = new ClaimsIdentity(claims, "WapIdentity");

                context.User = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(context.User, new AuthenticationProperties(), "WapScheme");
                AuthenticateResult.Success(ticket);
            }

            await _next(context);
        }
    }
}
