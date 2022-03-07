using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using VietCapital.Partner.F5Seconds.Infrastructure.Identity.Contexts;

[assembly: HostingStartup(typeof(VietCapital.Partner.F5Seconds.WebMvc.Areas.Identity.IdentityHostingStartup))]
namespace VietCapital.Partner.F5Seconds.WebMvc.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}