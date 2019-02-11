using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SecteEcoPlus.Areas.Identity.Data;
using SecteEcoPlus.Models;

[assembly: HostingStartup(typeof(SecteEcoPlus.Areas.Identity.IdentityHostingStartup))]
namespace SecteEcoPlus.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<WebsiteContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UsersContextConnection")));

                services.AddDefaultIdentity<SecteUser>()
                .AddEntityFrameworkStores<WebsiteContext>()
                .AddUserStore<SecteUserStore>()
                .AddUserManager<SecteUserManager>();

                //services.AddScoped<DbContext, WebsiteContext>();
                //services.AddTransient<IUserStore<SecteUser>, SecteUserStore>();
                services.TryAddScoped<SecteUserStore>();
                //services.TryAddScoped<UserManager<SecteUser>, SecteUserManager>();
            });
        }
    }
}