using System;
using System.IO;
using System.Reflection;
using DotNetCoreRestfulAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace DotNetCoreRestfulAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StockContext>(opt => opt.UseInMemoryDatabase("StocksDB"));
            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // config SwaggerGen
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                        Title = "Stock Market API",
                        Version = "v1",
                        Description = "An example Restful API using ASP.NET Core",
                        Contact = new Contact
                        {
                            Name = "Mateus Nascimento",
                            Url = "https://github.com/MateusNascimento"
                        }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // configs Swagger and SwaggerUI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock Market API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
