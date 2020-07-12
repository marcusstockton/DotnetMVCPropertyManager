using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Website.Data;
using Website.Interfaces;
using Website.Models;
using Website.Services;
using Website.Profiles;
using System;

namespace Website
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
            if (Environment.MachineName == "DEVELOPER-06")
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options
                    .UseLazyLoadingProxies()
                    .UseSqlite("Data Source=PropertyManager.db"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
           {
               options.User.RequireUniqueEmail = true;
           })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PropertyProfile());
                mc.AddProfile(new AddressProfile());
                mc.AddProfile(new TenantProfile());
                mc.AddProfile(new PortfolioProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddMvc();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.TryAddScoped<IPortfolioService, PortfolioService>();
            services.TryAddScoped<IPropertyDocumentService, PropertyDocumentService>();
            services.TryAddScoped<IPropertyImageService, PropertyImageService>();
            services.TryAddScoped<IPropertyService, PropertyService>();
            services.TryAddScoped<ITenantService, TenantService>();

            services.TryAddTransient<IEmailSender, EmailService>();
            services.TryAddTransient<DataSeeder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
                seeder.SeedData().GetAwaiter().GetResult();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}