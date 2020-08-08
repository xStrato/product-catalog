using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductCatalog.Data;

namespace ProductCatalog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();

            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer("Server=localhost,1433;Database=ProductCalalog;User ID=SA;Password=9p8o7i6u5y$#@"));

            services.AddSwaggerGen(x=>{
                x.SwaggerDoc("v1", new OpenApiInfo { Title="Product Catalog", Version="v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            
            app.UseMvc();
            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUI(x=>{
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog - V1");
            });
        }
    }
}