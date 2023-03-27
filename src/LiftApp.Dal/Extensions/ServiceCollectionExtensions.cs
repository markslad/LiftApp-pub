using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftApp.Dal.Contexts;
using LiftApp.Dal.Interfaces;
using LiftApp.Dal.UnitsOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace LiftApp.Dal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDataAccessLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IMainUnitOfWork, MainUnitOfWork>();
        }
    }
}
