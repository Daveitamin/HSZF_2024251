using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YY6VGC_HSZF_2024251.Application;
using YY6VGC_HSZF_2024251.Model;
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
                services.AddSingleton<IGPDataProvider, GPDataProvider>();
                services.AddSingleton<IDirectoryManager, DirectoryManager>();
            }).Build();
            host.Start();
            using IServiceScope ServiceProvider = host.Services.CreateScope();
            AppDbContext apdv = host.Services.GetRequiredService<AppDbContext>();
            IDirectoryManager directoryManager = host.Services.GetRequiredService<IDirectoryManager>();
            directoryManager.CreateRaceFolders();


            //Új pilóta felvétele
            var driver = new Drivers();
            Console.WriteLine("Add meg egy sofőr nevét:");
            driver.name = Console.ReadLine();
            Console.WriteLine($"Add meg {driver.name} csapatát: ");
            driver.team = Console.ReadLine();
            Console.WriteLine($"Add meg, hogy {driver.name}-nek hány pontja van az idei szezonban: ");
            driver.points = int.Parse(Console.ReadLine());


            //Meglévő pilóta törlése
            Console.WriteLine("Add meg a törölni kívánt pilóta id-ját: ");
            int deleteId = int.Parse(Console.ReadLine());

            //Meglévő pilóta csapata vagy pontja változtatása
            string driverEditName;
            Console.WriteLine("Add meg a pilóta nevét(HELYESEN!), akinek a csapatát vagy pontját szeretnéd változtatni.");
            driverEditName = Console.ReadLine();
            Console.WriteLine($"Add meg {driverEditName} új csapatát: ");
            string driverEditNewTeam = Console.ReadLine();
            Console.WriteLine($"Add meg {driverEditName} szerzett pontjait: ");
            int driverEditPointIncrease = int.Parse(Console.ReadLine());


            //Új verseny felvétele
            var newRace = new GrandPrixes();
            Console.WriteLine("Add meg a rögzíteni kívánt verseny helyszínét: ");
            newRace.Location = Console.ReadLine();
            Console.WriteLine("Add meg a verseny dátumát(Ilyen formában: 2023-07-23): ");
            newRace.Date = DateTime.Parse(Console.ReadLine());
            string[] podiumNames = new string[3];
            Console.WriteLine("Add meg az 1. helyezett nevét: ");
            podiumNames[0] = Console.ReadLine();
            Console.WriteLine("Add meg az 2. helyezett nevét: ");
            podiumNames[1] = Console.ReadLine();
            Console.WriteLine("Add meg az 3. helyezett nevét: ");
            podiumNames[2] = Console.ReadLine();

            //Meglévő verseny törlése
            Console.WriteLine("Add meg a törölni kívánt verseny id-ját: ");
            int deleteRaceId = int.Parse(Console.ReadLine());

            //Meglévő verseny eredményének módosítása
            Console.WriteLine("Add meg a módosítani kívánt verseny id-ját");
            int modifyRaceId = int.Parse(Console.ReadLine());
            string[] modifyRacePodiumNames = new string[3];
            Console.WriteLine("Kérlek add meg az új 1. helyezettet:");
            modifyRacePodiumNames[0] = Console.ReadLine();
            Console.WriteLine("Kérlek add meg az új 2. helyezettet:");
            modifyRacePodiumNames[1] = Console.ReadLine();
            Console.WriteLine("Kérlek add meg az új 3. helyezettet:");
            modifyRacePodiumNames[2] = Console.ReadLine();
        }
    }
}
