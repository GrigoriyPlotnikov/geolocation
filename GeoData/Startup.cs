using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeoData.Contracts;
using GeoData.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GeoData
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            using var sp = services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

            // IMPORTANT: Don't resolve singleton and instance registrations from this service provider!
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Startup>();

            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                logger.LogInformation($"Current directory is {currentDirectory}");

                services
                .AddOptions()
                .Configure<DbSettings>(Configuration.GetSection($"AppSettings:DbSettings"))
                .AddSingleton<IGeoIp, Db.GeoIp>()
                .AddSwaggerGen()
                .AddMvcCore()
                .AddApiExplorer();

                services.AddSpaStaticFiles(conf =>
                {
                    conf.RootPath = "GeoApp/dist";
                });
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception occurred while configuring services");

                throw;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                    .UseSwagger()
                    .UseSwaggerUI();
            }

            app.UseRouting()
                .UseStaticFiles()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                })
                .UseSpaStaticFiles();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "GeoApp";

                if (env.IsDevelopment())
                {
                    //spa.UseReactDevelopmentServer(npmScript: "build:hotdev");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8083");
                }
            });
        }
    }
}
