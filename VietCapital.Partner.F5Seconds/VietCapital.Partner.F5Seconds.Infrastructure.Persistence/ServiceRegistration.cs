using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Contexts;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repositories;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repository;

namespace VietCapital.Partner.F5Seconds.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            string appConStr = configuration.GetConnectionString("DefaultConnection");
            if (isProduction)
            {
                appConStr = Environment.GetEnvironmentVariable("DB_URI_APPLICATION");
            }
            var serverVersion = new MySqlServerVersion(new Version(10, 5, 10));
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                appConStr, serverVersion,
                //b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
                b => b.MigrationsAssembly("VietCapital.Partner.F5Seconds.WebMvc")));
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient(typeof(IGatewayHttpClientService), typeof(GatewayHttpClientRepository));

            services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();
            services.AddTransient<ICategoryRepositoryAsync, CategoryRepositoryAsync>();
            services.AddTransient<IVoucherTransactionRepositoryAsync, VoucherTransactionRepositoryAsync>();

            #endregion
        }
    }
}
