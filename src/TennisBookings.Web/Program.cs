using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TennisBookings.Web.Data;

namespace TennisBookings.Web
{
    public class Program
    {
        public static async Task Main (string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var hostingEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                var appLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();

                if (hostingEnvironment.IsDevelopment())
                {
                    var ctx = serviceProvider.GetRequiredService<TennisBookingDbContext>();
                    await ctx.Database.MigrateAsync(appLifetime.ApplicationStopping);

                    try
                    {
                        var userManager = serviceProvider.GetRequiredService<UserManager<TennisBookingsUser>>();
                        var roleManager = serviceProvider.GetRequiredService<RoleManager<TennisBookingsRole>>();

                        await SeedData.SeedUsersAndRoles(userManager, roleManager);
                    }
                    catch (Exception ex)
                    {
                        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("UserInitialisation");
                        logger.LogError(ex, "Failed to seed user data");
                    }
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    if (ctx.HostingEnvironment.IsProduction())
                    {
                        var config = builder.Build();
                        var tokenProvider = new AzureServiceTokenProvider();

                        var kvClient = new KeyVaultClient((authority, resource, scope) =>
                            tokenProvider.KeyVaultTokenCallback(authority, resource, scope));

                        builder.AddAzureKeyVault(config["KeyVault:BaseUrl"], kvClient,
                            new DefaultKeyVaultSecretManager());
                    }
                })
                ;
    }
}
