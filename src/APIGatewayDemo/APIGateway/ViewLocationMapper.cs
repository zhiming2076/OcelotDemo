using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Ocelot.Configuration.File;

namespace APIGateway.Admin
{
    internal class ViewLocationMapper : IViewLocationExpander
    {
        private readonly IEnumerable<string> _preCompiledViewLocations;

        public ViewLocationMapper()
        {
            _preCompiledViewLocations = new[]
                                            {
                                                "/Admin/Views/{1}/{0}.cshtml", "/Admin/Views/Shared/{1}/{0}.cshtml",
                                                "/Admin/Views/Shared/{0}.cshtml", "/Admin/Views/{0}.cshtml"
                                            };
        }

        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue("APIGatewayAdmin", out string isEditor)
                && isEditor == bool.TrueString) return _preCompiledViewLocations;

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context.ActionContext.ActionDescriptor.MatchesNamespaceInRoute(context))
                context.Values.Add("APIGatewayAdmin", bool.TrueString);
        }
    }

    internal static class RouteContextExtensions
    {
        public static bool MatchesNamespaceInRoute(this ActionDescriptor action, RouteContext routeContext)
        {
            return MatchesNamespaceInRoute(action, routeContext.RouteData);
        }

        public static bool MatchesNamespaceInRoute(
            this ActionDescriptor action,
            ViewLocationExpanderContext viewLocationExpanderContext)
        {
            return MatchesNamespaceInRoute(action, viewLocationExpanderContext.ActionContext.RouteData);
        }

        private static bool MatchesNamespaceInRoute(ActionDescriptor action, RouteData routeData)
        {
            var dataTokenNamespace = (string)routeData.DataTokens.FirstOrDefault(dt => dt.Key == "Namespace").Value;
            var actionNamespace = ((ControllerActionDescriptor)action).MethodInfo.DeclaringType.Namespace;

            return dataTokenNamespace == actionNamespace;
        }
    }

    public static class FileReRouteExtensions
    {
        public static string GetId(this FileReRoute fileReRoute)
        {
            return fileReRoute == null
                       ? string.Empty
                       : $"{fileReRoute.DownstreamScheme}{fileReRoute.DownstreamHostAndPorts[0].Host}{fileReRoute.DownstreamHostAndPorts[0].Port}{fileReRoute.DownstreamPathTemplate}"
                           .Replace('/', '_');
        }
    }
}