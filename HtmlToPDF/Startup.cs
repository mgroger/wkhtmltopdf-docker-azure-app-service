using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HtmlToPDF
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //NOTE - Unable to load shared library '/app/wkhtmltox\v0.12.5\64 bit\libwkhtmltox' or one of its dependencies.
            //var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
            //var wkHtmlToPdfPath = Path.Combine(HostingEnvironment.ContentRootPath, $"wkhtmltox\\v0.12.5\\{architectureFolder}\\libwkhtmltox");

            //CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            //context.LoadUnmanagedLibrary(wkHtmlToPdfPath);


            // Add converter to DI
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
