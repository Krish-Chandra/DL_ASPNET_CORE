using DL_Core_WebAPP_Release.Middleware;
using DL_Core_WebAPP_Release.Migrations;
using DL_Core_WebAPP_Release.Models;
using DL_Core_WebAPP_Release.Models.Services;
using DL_Using_DotNet_Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DL_Core_WebAPP_Release
{
    public class Startup
    {

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSession();

            var connection = @"Server=(localdb)\mssqllocaldb;Database=DL.AspNetCore.10;Trusted_Connection=True;";
            services.AddDbContext<DLContext>(options => options.UseSqlServer(connection));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Admin/Home/AccessDenied";
            })
            .AddEntityFrameworkStores<IdentityContext>();
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connection));


            services.AddSingleton<UserNameFromUserIdService>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ManageAdminUsers", policy => policy.RequireClaim("AdminUsers", "Allowed"));
                options.AddPolicy("ManageBooks", policy => policy.RequireClaim("Books", "Allowed"));
                options.AddPolicy("ManageAuthors", policy => policy.RequireClaim("Authors", "Allowed"));
                options.AddPolicy("ManageCategories", policy => policy.RequireClaim("Categories", "Allowed"));
                options.AddPolicy("ManagePublishers", policy => policy.RequireClaim("Publishers", "Allowed"));
                options.AddPolicy("ManageRequests", policy => policy.RequireClaim("Requests", "Allowed"));
                options.AddPolicy("ManageIssues", policy => policy.RequireClaim("Issues", "Allowed"));
                options.AddPolicy("ManageRoles", policy => policy.RequireClaim("Roles", "Allowed"));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSession(); //This must come before UseMvc. Else, you'll get InvalidOperationException
            app.UseIdentity();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Books}/{action=Index}"
                    //defaults: new { action = "Index" });
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "library", action = "Index" }
                );

            });

            //Thanks: Scott Allen for this middleware
            app.UseNodeModules(env);
            DatabaseSeeder.SeedTheDatabase(app.ApplicationServices).Wait();
            app.UseStaticFiles();
        }
    }

}
