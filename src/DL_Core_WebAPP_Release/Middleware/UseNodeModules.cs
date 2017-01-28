using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace DL_Core_WebAPP_Release.Middleware
{
    public static class FrontEndFiles
    {
        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app, IHostingEnvironment env)
        {
            var path = Path.Combine(env.ContentRootPath, "node_modules");
            var provider = new PhysicalFileProvider(path);

            var options = new StaticFileOptions();
            options.RequestPath = "/node_modules";
            options.FileProvider = provider;

            app.UseStaticFiles(options);
            return app;
        }

    }
}
