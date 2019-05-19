using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.StaticFiles;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;

namespace WebApplication7
{
    public class Startup
    {
        private   IHostingEnvironment CurrentEnviroment { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            CurrentEnviroment = env;
            var builder = new ConfigurationBuilder()
                        .SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();

            Configuration = builder.Build( );
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (CurrentEnviroment.IsDevelopment() || CurrentEnviroment.IsStaging() || CurrentEnviroment.EnvironmentName == "TEST")
            {
                services.AddMiniProfiler(options => options.RouteBasePath = "/profiler");
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info()
                    {
                        Description = "Benim Apim",
                        Title = "v1",
                        Version = "v1"

                    });
                });
            }
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment() || env.IsStaging() || env.EnvironmentName == "TEST")
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {

                    c.RoutePrefix = "api-doc";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Api V1");
                    c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("WebApplication7.SwaggerIndex.html"); 
                });

                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction())
            {

            }

            app.UseMiniProfiler();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc();
            app.UseCors();
        }
    }
}
