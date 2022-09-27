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
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Website.Data;
using Website.Interfaces;
using Website.Models;
using Website.Profiles;
using Website.Services;

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
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                .UseLazyLoadingProxies()
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

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
                mc.AddProfile(new DocumentTypeProfile());
                mc.AddProfile(new PropertyDocumentProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllersWithViews(options => {
                options.CacheProfiles.Add("Default30", new Microsoft.AspNetCore.Mvc.CacheProfile { Duration = 30, Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Client });
                options.CacheProfiles.Add("Default60", new Microsoft.AspNetCore.Mvc.CacheProfile { Duration = 60, Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Client });
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddMvc();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.TryAddScoped<IPortfolioService, PortfolioService>();
            services.TryAddScoped<IPropertyDocumentService, PropertyDocumentService>();
            services.TryAddScoped<IPropertyImageService, PropertyImageService>();
            services.TryAddScoped<IPropertyService, PropertyService>();
            services.TryAddScoped<ITenantService, TenantService>();
            services.TryAddScoped<IJobTitleService, JobTitleServiceCache>();

            

            services.TryAddTransient<IEmailSender, EmailService>();
            services.TryAddTransient<DataSeeder>();

            services.AddResponseCaching();
            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeeder seeder, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                var path = Directory.GetCurrentDirectory();
                loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                seeder.SeedData(false).Wait();
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

            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(30)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

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