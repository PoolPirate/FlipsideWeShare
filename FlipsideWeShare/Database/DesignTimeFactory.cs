using Common.Extensions;
using FlipsideWeShare.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace FlipsideWeShare.Database;
internal class DesignTimeFactory : IDesignTimeDbContextFactory<BountyContext>
{
    public BountyContext CreateDbContext(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        return host.Services.CreateScope().ServiceProvider.GetRequiredService<BountyContext>();
    }


    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var assembly = Assembly.GetExecutingAssembly();

        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("appsettings.json");
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddApplication(hostContext.Configuration, options =>
                {
                    options.UseServiceLevels = true;
                    options.ValidateServiceLevelsOnInitialize = true;
                }, assembly);

                services.AddDbContext<BountyContext>((services, options) =>
                {
                    var dbOptions = services.GetRequiredService<DbOptions>();
                    options.UseNpgsql(dbOptions.ConnectionString);
                });

                services.AddSingleton<HttpClient>();
            });
    }
}
