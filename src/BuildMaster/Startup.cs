using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BuildMaster.Infrastructure;

namespace LibCloud.Core
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            //Use a MS SQL Server database
            var sqlConnectionString = AppConfigProvider.Instance.GetConnectionString();

            Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");

            Console.WriteLine($"Connection string: {sqlConnectionString}");

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

            AppConfigProvider.Instance.Configure(args);

            ConfigureServices(ServiceCollectionProvider.Instance.Collections);

            ServiceCollectionProvider.Instance.Provider.GetService<ApplicationDbContext>().Database.Migrate();

            Console.WriteLine("Done");
        }
    }
}