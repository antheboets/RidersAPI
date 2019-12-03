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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Riders.Data;
using Riders.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

namespace Riders
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
            string connectionString = Configuration.GetConnectionString("RidersContext");
            services.AddDbContext<Data.RidersContext>(options => options.UseSqlServer(connectionString));
            services.AddControllers();
            services.AddCors();
            //services.AddSingleton<IPostConfigureOptions<BasicAuthenticationOptions>, BasicAuthenticationPostConfigureOptions>();
            services.AddScoped<IRideRepository, RideRepository>();
            services.AddDbContext<RidersContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("RidersContext")));
            //services.AddAuthentication("Basic").AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>("Basic", null);
            //services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme).AddBasic<AuthenticationService>(o => });
            services.AddAuthentication(options =>
            {
                // the scheme name has to match the value we're going to use in AuthenticationBuilder.AddScheme(...)
                options.DefaultAuthenticateScheme = "Custom Scheme";
                options.DefaultChallengeScheme = "Custom Scheme";
            })
            .AddCustomAuth(o => { });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RidersContext context)
        {
            context.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
