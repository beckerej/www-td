using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using www_td.Database.Models;

namespace www_td.MachineStatusService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<Worker>();
            services.AddEntityFrameworkNpgsql().AddDbContext<WebApiContext>(opt =>
                opt.UseNpgsql(hostContext.Configuration.GetConnectionString("WebApiConnection"))
                   .UseSnakeCaseNamingConvention());
        });
    }
}
