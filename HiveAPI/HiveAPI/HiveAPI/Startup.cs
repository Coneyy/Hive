using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HiveAPI.Models;
using System.IO;
using HiveAPI.Data;

namespace HiveAPI
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(env.ContentRootPath)
                              .AddJsonFile("appsettings.json");
            Configuration = builder.Build();

        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=Hive.Players;Trusted_Connection=True;";
            services.AddMvc();
            //services.AddDbContext<HiveApiContext>(opt => opt.UseInMemoryDatabase());
            services.AddDbContext<HiveApiContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));            
            services.AddMvc();
        }



        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}

