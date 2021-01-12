using Microsoft.AspNetCore.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: HostingStartup(typeof(MisteriosDaNaturaleza.Areas.Identity.IdentityHostingStartup))]
namespace MisteriosDaNaturaleza.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((contexto, services) => {
            });
        }
    }
}
