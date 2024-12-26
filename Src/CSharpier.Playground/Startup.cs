using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CSharpier.Playground;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/build";
        });
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        ILoggerFactory loggerFactory
    )
    {
        loggerFactory.AddFile("App_Data/logs_{Date}.txt");

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();
        app.UseSpaStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}"
            );
        });

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
                spa.Options.SourcePath = "ClientApp";
                spa.Options.DevServerPort = 3000;
                spa.UseReactDevelopmentServer(npmScript: "start");
            }
        });
    }
}
