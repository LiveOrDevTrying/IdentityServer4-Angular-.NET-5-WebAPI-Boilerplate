using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TheMonitaur.WebApps.Landing.Areas.Identity.IdentityHostingStartup))]
namespace TheMonitaur.WebApps.Landing.Areas.Identity
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