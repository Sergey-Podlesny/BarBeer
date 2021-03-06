using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarBeer.Controllers;
using Microsoft.Extensions.Configuration;
using BarBeer.Context;
using Microsoft.EntityFrameworkCore;
using BarBeer.Services;
using BarBeer.Services.Implementations;
using System.Text.Json.Serialization;

namespace BarBeer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BarBeerContext>(options =>
                options.UseSqlServer(connection));
            services.AddControllers();
            services.AddScoped<IUserService, UserService>()
                    .AddScoped<IBarService, BarService>()
                    .AddScoped<UserController>()
                    .AddScoped<BarController>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
