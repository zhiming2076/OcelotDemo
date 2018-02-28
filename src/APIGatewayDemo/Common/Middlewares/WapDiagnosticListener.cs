
using Common.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Middlewares
{
    public class WapDiagnosticListener
    {
        private readonly IHubContext<WapDiagnosticHub> repHub;
        public WapDiagnosticListener(IApplicationBuilder app)
        {
            this.repHub = app.ApplicationServices.GetService<IHubContext<WapDiagnosticHub>>();
        }

        [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareStarting")]
        public virtual void OnMiddlewareStarting(HttpContext httpContext, string name)
        {
            repHub.Clients.All.InvokeAsync("OnSendDiagnostics", $"MiddlewareStarting: {name}; {httpContext.Request.Path}<br />").Wait();
        }

        [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareException")]
        public virtual void OnMiddlewareException(Exception exception, string name)
        {
            repHub.Clients.All.InvokeAsync("OnSendDiagnostics", $"MiddlewareException: {name}; {exception.Message}<br />").Wait();
        }

        [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareFinished")]
        public virtual void OnMiddlewareFinished(HttpContext httpContext, string name)
        {
            repHub.Clients.All.InvokeAsync("OnSendDiagnostics", $"MiddlewareFinished: {name}; {httpContext.Response.StatusCode}<br />").Wait();
        }
    }
}
