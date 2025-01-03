using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebAPI.Models;

namespace WebAPI
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
            services.AddControllers();

            // Configure the database context
            services.AddDbContext<DonationDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));

            // Configure Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DonationDBContext>()
                .AddDefaultTokenProviders();

            // Configure IdentityServer
            services.AddIdentityServer()
                .AddDeveloperSigningCredential() // For development purposes
                .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
                .AddInMemoryApiScopes(IdentityConfig.GetApiScopes())
                .AddInMemoryClients(IdentityConfig.GetClients())
                .AddAspNetIdentity<IdentityUser>(); // Integrate Identity with IdentityServer

            // Register Custom Cors Policy Service
            services.AddSingleton<ICorsPolicyService, CustomCorsPolicyService>();

            // Add Authorization
            services.AddAuthorization(); // Add this line

            // Add CORS
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
            options.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Add the authentication and authorization middlewares
            app.UseAuthentication(); // Ensure this is added
            app.UseAuthorization();  // Ensure this is added

            app.UseIdentityServer(); // Add IdentityServer middleware

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Add roles
                if (!roleManager.RoleExistsAsync("Maker").Result)
                {
                    roleManager.CreateAsync(new IdentityRole("Maker")).Wait();
                }

                if (!roleManager.RoleExistsAsync("Checker").Result)
                {
                    roleManager.CreateAsync(new IdentityRole("Checker")).Wait();
                }

                // Add Maker user
                if (userManager.FindByNameAsync("maker").Result == null)
                {
                    var maker = new IdentityUser { UserName = "maker", Email = "maker@example.com", EmailConfirmed = true };
                    userManager.CreateAsync(maker, "Maker@123").Wait();
                    userManager.AddToRoleAsync(maker, "Maker").Wait();
                }

                // Add Checker user
                if (userManager.FindByNameAsync("checker").Result == null)
                {
                    var checker = new IdentityUser { UserName = "checker", Email = "checker@example.com", EmailConfirmed = true };
                    userManager.CreateAsync(checker, "Checker@123").Wait();
                    userManager.AddToRoleAsync(checker, "Checker").Wait();
                }
            }
        }
    }

}
