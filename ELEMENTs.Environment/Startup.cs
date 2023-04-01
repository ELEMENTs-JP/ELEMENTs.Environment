using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELEMENTS;
using ELEMENTS.Data.SQLite;
using ELEMENTS.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ELEMENTs.Environment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            // Authentication Handler (Cookie) 
            // https://www.youtube.com/watch?v=4QBa0hRI4ds&list=PLgRlicSxjeMOxypAEL2XqIc2m_gPmoVN-&index=8 
            services.AddAuthentication().AddCookie("tspCookieAuth", options =>
                {
                    options.Cookie.Name = "tspCookieAuth";
                    options.LoginPath = "/Account/Login";
                });

            // TODO: Security - Security Data Context
            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

            // Database Service 
            services.AddSingleton<ISqlDatabaseService, SQLiteService>();

            // Security Service
            services.AddSingleton<ISecurityService, SecurityService>();

            // App Service 
            services.AddSingleton<IAppRepository, AppRepository>();

            // File Service 
            services.AddSingleton<IFileDragDropService, FileDragDropUploadService>();
            services.AddSingleton<IFileUploadService, FileUploadService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Security Middleware 
            // https://www.youtube.com/watch?v=4QBa0hRI4ds&list=PLgRlicSxjeMOxypAEL2XqIc2m_gPmoVN-&index=8 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
