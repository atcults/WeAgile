using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BuildMaster.Infrastructure;

namespace LibCloud.Core
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void ConfigureServices(IServiceCollection services)
        {
            //Use a MS SQL Server database
            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddEntityFramework().AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("BuildMaster")
                )
            );
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting up application");

            var services = new ServiceCollection();

            Configuration = new ConfigurationBuilder()
                           .AddCommandLine(args)
                           .AddJsonFile($"appsettings.json", optional: false)
                           .AddEnvironmentVariables()
                           .Build();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();

            Console.WriteLine("Done");

        }
    }


}