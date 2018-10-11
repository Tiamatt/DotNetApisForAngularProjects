using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using DotNetApisForAngularProjects.HomeCuisineDbModels;
using DotNetApisForAngularProjects.PapaJohnsCloneDbModels;

namespace DotNetApisForAngularProjects
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // DB connection
            var HomeCuisineDbConn = Configuration.GetConnectionString("HomeCuisineDbConn");
            services.AddDbContext<HomeCuisineDbContext>(options => options.UseSqlServer(HomeCuisineDbConn));
            var PapaJohnsCloneDbConn = Configuration.GetConnectionString("PapaJohnsCloneDbConn");
            services.AddDbContext<PapaJohnsCloneDbContext>(options => options.UseSqlServer(PapaJohnsCloneDbConn));

            // ********************
            // Setup CORS
            // ********************
            
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            //corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url. Don't add a forward slash on the end!
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            
            // ********************
            // USE CORS - might not be required.
            // ********************
            app.UseCors("SiteCorsPolicy");
        }
    }
}

// from local
// "ConnectionStrings": {
//   "HomeCuisineDbConn": "Server=DESKTOP-O93KUA7\\SQLEXPRESS;Database=HomeCuisineDb;User Id=HomeCuisineDb_Admin;Password=H0meCu1s1ne62983;Trusted_Connection=True;",
//   "PapaJohnsCloneDbConn": "Server=DESKTOP-O93KUA7\\SQLEXPRESS;Database=PapaJohnsCloneDb;User Id=PapaJohnsCloneDb_admin;Password=PapaJohns933455;Trusted_Connection=True;"
// },
// from smarterasp.net hosting
//   "ConnectionStrings": {
//     "HomeCuisineDbConn": "Data Source=SQL5037.site4now.net;Initial Catalog=DB_A41982_HomeCuisineDb;User Id=DB_A41982_HomeCuisineDb_admin;Password=H0meCu1s1ne62983;",
//     "PapaJohnsCloneDbConn":  "Data Source=SQL5037.site4now.net;Initial Catalog=DB_A41982_PapaJohnsCloneDb;User Id=DB_A41982_PapaJohnsCloneDb_admin;Password=PapaJohns933455;"
//   },