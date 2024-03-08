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

            services.AddAutoMapper(typeof(Program));

            services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("Default5", new Microsoft.AspNetCore.Mvc.CacheProfile { Duration = 5, Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Any });
                options.CacheProfiles.Add("Default30", new Microsoft.AspNetCore.Mvc.CacheProfile { Duration = 30, Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Any });
                options.CacheProfiles.Add("Default60", new Microsoft.AspNetCore.Mvc.CacheProfile { Duration = 60, Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Any });
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

            services.AddHttpClient("postcodesioClient", client =>
            {
                var url = Configuration.GetSection("ThirdPartyClients:PostcodesIo").GetValue<string>("BaseUrl");
                client.BaseAddress = new Uri(url);
            });

            services.AddHttpClient("hereApiLookup", client =>
            {
                var url = Configuration.GetSection("ThirdPartyClients:HereApiLookup").GetValue<string>("BaseUrl");
                client.BaseAddress = new Uri(url);
            });

            services.AddHttpClient("hereApiImages", client =>
            {
                var url = Configuration.GetSection("ThirdPartyClients:HereApiImages").GetValue<string>("BaseUrl");
                client.BaseAddress = new Uri(url);
            });

            services.AddHttpClient("hereApiAutosuggest", client =>
            {
                var url = Configuration.GetSection("ThirdPartyClients:HereApiAutoSuggest").GetValue<string>("BaseUrl");
                client.BaseAddress = new Uri(url);
            });

            services.TryAddTransient<IEmailSender, EmailService>();
            services.TryAddTransient<DataSeeder>();

            services.AddMemoryCache((o) =>
            {
                o.CompactionPercentage = 0.9;
                o.ExpirationScanFrequency = TimeSpan.FromMinutes(1.0);
                o.SizeLimit = 1024 * 1024 * 8; // 8 mb
            });

            services.AddResponseCaching();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeeder seeder, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                var path = Directory.GetCurrentDirectory();
                loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

                app.UseDeveloperExceptionPage();
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

            app.UseHealthChecks("/healthz");

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