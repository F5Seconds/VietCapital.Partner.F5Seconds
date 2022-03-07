using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VietCapital.Partner.F5Seconds.Infrastructure.Identity.Contexts;
using VietCapital.Partner.F5Seconds.Infrastructure.Identity.Models;

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