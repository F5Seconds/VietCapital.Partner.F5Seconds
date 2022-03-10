using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VietCapital.Partner.F5Seconds.Infrastructure.Identity.Contexts;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Contexts;

namespace VietCapital.Partner.F5Seconds.WebMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var applicationDb = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var IdentityDb = scope.ServiceProvider.GetRequiredService<IdentityContext>();
                applicationDb.Database.Migrate();
                IdentityDb.Database.Migrate();
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
