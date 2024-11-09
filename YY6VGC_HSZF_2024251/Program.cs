using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var host = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<AppDbContext>();
            }).Build();
            host.Start();
            using IServiceScope ServiceProvider = host.Services.CreateScope();
            AppDbContext apdv = host.Services.GetRequiredService<AppDbContext>();
        }
    }
}
