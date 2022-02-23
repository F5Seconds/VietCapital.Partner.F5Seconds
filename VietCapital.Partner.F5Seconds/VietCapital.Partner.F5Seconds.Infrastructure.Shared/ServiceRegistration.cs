using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Domain.Settings;
using VietCapital.Partner.F5Seconds.Infrastructure.Shared.Services;

namespace VietCapital.Partner.F5Seconds.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
