using LiftApp.Dal.Contexts;
using LiftApp.Dal.Interfaces;
using LiftApp.Dal.UnitsOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Tests.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void RegisterServicesForDbContextTesting(this IServiceCollection services)
        {
            Dictionary<string, string> inMemoryConfigSettings = new()
            {
                { "ConnectionStrings:DefaultConnection", "Host=localhost;Database=lift-app-db-testing;Username=postgres;Password=pass" }
            };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemoryConfigSettings).Build();
            services.AddScoped<IConfiguration>(_ => configuration);

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
