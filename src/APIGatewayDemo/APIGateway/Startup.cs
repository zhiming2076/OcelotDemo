using APIGateway.Admin;
using APIGatewayDemo.Common.Logging;
using APIGatewayDemo.Common.Middlewares;
using Common.Hubs;
using Common.Middlewares;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Configuration.Provider;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.DownstreamRouteFinder.UrlMatcher;
using Ocelot.Middleware;
using System;
using System.Diagnostics;
using System.Text.Encodings.Web;

namespace APIGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IWapLoggerFactory, WapLoggerFactory>();

            services.AddAuthentication(options =>
            {
                options.AddScheme<WapAuthenticationHandler>("Token", "WapAuthentication");
            });

            services.AddSignalR();

            services.AddOcelot()
                .AddOpenTracing(option =>
                {
                    option.CollectorUrl = "http://localhost:9618";
                    option.Service = "Ocelot";
                })
                .AddAdministration("/administration", "REALLYHARDPASSWORD");

            services.AddMvc();
            services.Configure<RazorViewEngineOptions>(opt => {
                opt.ViewLocationExpanders.Add(new ViewLocationMapper());
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, DiagnosticListener diagnosticListener)
        {
            var listener = new WapDiagnosticListener(app);
            diagnosticListener.SubscribeWithAdapter(listener);

            #region register admin branch
            app.Map("/admin", subApp => {
                subApp.UseStaticFiles();
                subApp.UseMvc( 
                    routes =>
                    {
                        routes.MapRoute(
                                        "Admin",
                                        $"{{controller=Home}}/{{action=Index}}/{{id?}}",
                                        null,
                                        null,
                                        new { Namespace = "APIGateway.Controllers" });
                    });
            });
            #endregion

            #region register diagnostic branch
            app.Map("/diagnostic", subApp => {
                subApp.UseStaticFiles();
                subApp.UseMvc(
                    routes =>
                    {
                        routes.MapRoute(
                                        "Diagnostic",
                                        $"{{controller=Diagnostic}}/{{action=Index}}/{{id?}}",
                                        null,
                                        null,
                                        new { Namespace = "APIGateway.Controllers" });
                    });
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<WapDiagnosticHub>("wapDiagnosticHub");
            });
            #endregion

            #region register diagnostic branch
            app.Map("/logging", subApp => {
                subApp.UseStaticFiles();
                subApp.UseMvc(
                    routes =>
                    {
                        routes.MapRoute(
                                        "Logging",
                                        $"{{controller=Logging}}/{{action=Index}}/{{id?}}",
                                        null,
                                        null,
                                        new { Namespace = "APIGateway.Controllers" });
                    });
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<WapDiagnosticHub>("wapDiagnosticHub");
            });
            #endregion

            await app.UseOcelot();
        }
    }
}
