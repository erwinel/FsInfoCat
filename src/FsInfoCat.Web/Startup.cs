using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FsInfoCat.Web
{
    public class Startup
    {
        public const string CookieScheme = "FsInfoCat";
        public const string SettingsKey_ConnectionString = "FsInfoCat";
        public const string SettingsKey_DBPassword = "DBPassword";
        public const string UrlPath_ExceptionHandler = "/Home/Error";
        public const string UrlPath_AccessDeniedHandler = "/Accounts/AccessDenied";
        public const string UrlPath_LoginHandler = "/Accounts/Login";
        public const string UrlPath_LogoutHandler = "/Accounts/Logout";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // services.AddAntiforgery();
            services.AddAuthentication(CookieScheme) // Sets the default scheme to cookies
                .AddCookie(CookieScheme, options =>
                {
                    options.AccessDeniedPath = UrlPath_AccessDeniedHandler;
                    options.LoginPath = UrlPath_LoginHandler;
                    options.LogoutPath = UrlPath_LogoutHandler;
                });
            // services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureFsInfoCatCookie>();
            string connectionString = Configuration.GetConnectionString(SettingsKey_ConnectionString);
            string pwd = Configuration[SettingsKey_DBPassword];
            if (!String.IsNullOrEmpty(pwd))
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                builder.Password = pwd;
                connectionString = builder.ConnectionString;
            }

            services.AddDbContext<Data.FsInfoDataContext>(options =>
                options.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler(UrlPath_ExceptionHandler);
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
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
