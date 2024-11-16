using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
                services.AddSingleton<IRaceResultUpdate, RaceResultUpdate>();
                services.AddSingleton<IDatabaseManager, DatabaseManager>();

                //xml
                services.AddSingleton<IXMLDataProvider, XMLDataProvider>();
                services.AddSingleton<IXMLMethod, XMLMethod>();
                //txt
                //services.AddSingleton<ISeasonReportGenerator, SeasonReportGenerator>();

                //txt2.0
                services.AddSingleton<ISeasonReportGenerator, SeasonReportGenerator>();
                services.AddSingleton<ITxtWriter, TxtWriter>();

                //piloták listázása csökkenő pontszám szerint 
                services.AddSingleton<IDriverLister, DriverLister>();
                services.AddSingleton<IDriverDisplay, DriverDisplay>();

                //Versenydobogósok megjelenítése
                services.AddSingleton<IRacePodiumLister, RacePodiumLister>();
                services.AddSingleton<IPodiumDisplay, PodiumDisplay>();

                //A futamokon résztvevő versenyzők szűrése nemzetiség szerint
                services.AddSingleton<IDriverByNationalityLister, DriverByNationalityLister>();
                services.AddSingleton<IDriverNationalityDisplay, DriverNationalityDisplay>();

                //legjobb 5 versenyző legalább 2 versenyen
                services.AddSingleton<IBestFiveRacersDataProvider, BestFiveRacersDataProvider>();
                services.AddSingleton<IBestFiveRacersDisplay, BestFiveRacersDisplay>();

                //Egy adott csapat versenyzői összesen hány különböző versenyen értek el dobogós helyezést
                services.AddSingleton<ITeamDriverAnalyzerDataProvider, TeamDriverAnalyzerDataProvider>();
                services.AddSingleton<IDriverPodiumCountByTeamDisplay, DriverPodiumCountByTeamDisplay>();


            }).Build();
            host.Start();
            using IServiceScope ServiceProvider = host.Services.CreateScope();
            AppDbContext apdv = host.Services.GetRequiredService<AppDbContext>();
            IDirectoryManager directoryManager = host.Services.GetRequiredService<IDirectoryManager>();
            directoryManager.CreateRaceFolders();

            IXMLMethod xmlll = host.Services.GetRequiredService<IXMLMethod>();
            xmlll.SaveAllRacesToXml();

            //txt
            //var seasonReportGenerator = host.Services.GetRequiredService<ISeasonReportGenerator>();
            //seasonReportGenerator.GenerateSeasonSummary();

            //txt2.0
            var summaryProvider = host.Services.GetRequiredService<ISeasonReportGenerator>();
            var txtWriter = host.Services.GetRequiredService<ITxtWriter>();

            //pilota megjelenitése pont csökkenő
            var driverDiplay = host.Services.GetRequiredService<IDriverDisplay>();

            //versenydobogós megjelenítés
            var podiumDisplay = host.Services.GetRequiredService<IPodiumDisplay>();

            //A futamokon résztvevő versenyzők szűrése nemzetiség szerint

            var driverNationalityDisplayer = host.Services.GetRequiredService<IDriverNationalityDisplay>();

            //Legjobb 5 versenyző, akik legalább 2 különböző versenyen nyertek
            var fiveBest = host.Services.GetRequiredService<IBestFiveRacersDisplay>();

            //Adott csapat legjobb versenyzői podium helyezés
            var analyzer = host.Services.GetRequiredService<ITeamDriverAnalyzerDataProvider>();
            var outputHandler = host.Services.GetRequiredService<IDriverPodiumCountByTeamDisplay>();


            //var context = new AppDbContext(); ///nem kell de ha igen különben alatta apdv helyett context
            var winnerNotifier = new CustomEventHandler(apdv);
            //feliratkozás
            winnerNotifier.WinnerPilotFound += (sender, message) =>
            {
                Console.WriteLine($"Értesítés: {message}");
            };

            //winnerNotifier.CheckAndNotifyWinner();

            //txtiras



            IDatabaseManager databaseManager = host.Services.GetService<IDatabaseManager>();



            ////////////GUI////////////////
            ///betöltés
            DisplayLoadingAnimation("Kurva eget");


            bool exit = false;
            while (!exit)
            {
                int runningConfig = 0;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Formula 1 Database Center Ohio");
                Console.WriteLine("A folytatáshoz nyomj ENTER-t!");
                Console.ReadKey();
                Console.WriteLine("MENU:");
                Console.WriteLine("1. Új verseny létrehozása");
                Console.WriteLine("2. Versenyriport generálása a főmappába");
                Console.WriteLine("3. Versenyzők listázása pontszám szerint csökkenő sorrendben");
                Console.WriteLine("4. Egy adott verseny dobogósainak megjelenítése");
                Console.WriteLine("5. A futamokon résztvevő versenyzők szűrése nemzetiség szerint");
                Console.WriteLine("6. Legjobb 5 versenyző, akik legalább 2 különböző versenyen nyertek");
                Console.WriteLine("7. Meglévő pilóta törlése");
                Console.WriteLine("8. EXIT");
                Console.WriteLine("A választott menüpont: ");
                runningConfig = int.Parse(Console.ReadLine());
                
                switch (runningConfig)
                {
                    case 0:
                        
                        break;
                    case 1:
                        var newRace = new GrandPrixes();
                        Console.WriteLine("Add meg a verseny helyszínét: ");
                        newRace.Location = Console.ReadLine();
                        Console.WriteLine("Add meg a verseny dátumát(Ilyen formában: 2023-07-23): ");
                        newRace.Date = DateTime.Parse(Console.ReadLine());
                        Console.WriteLine("Add meg a pódiumon helyt foglaló versenyzőket a következő formában: ");
                        Console.WriteLine("pl.: (Max Verstappen, Carlos Sainz, Lewis Hamilton)");

                        string[] podiumNames = new string[3];
                        Console.WriteLine("1. helyezett neve:");
                        podiumNames[0] = Console.ReadLine();
                        Console.WriteLine("2. helyezett neve:");
                        podiumNames[1] = Console.ReadLine();
                        Console.WriteLine("3. helyezett neve:");
                        podiumNames[2] = Console.ReadLine();
                        newRace.Podium = podiumNames;
                        databaseManager.CreateRace(newRace);
                        winnerNotifier.CheckAndNotifyWinner();

                        break;
                    case 2:
                        Dictionary<string, int> teamsScores = summaryProvider.GetTeamScores();
                        Dictionary<string, int> driverScores = summaryProvider.GetDriverScores();
                        string filePath = Path.Combine("Formula1_2023", "SeasonSummary.txt");
                        txtWriter.WriteSeasonSummary(filePath, teamsScores, driverScores);
                        break;
                    case 3:
                        driverDiplay.DisplayDriversByPoints();
                        break;
                    case 4:
                        Console.WriteLine("Add meg a verseny helyszínét: ");
                        string requestLocation = Console.ReadLine();
                        podiumDisplay.ShodPodium(requestLocation);
                        break;
                    case 5:
                        Console.WriteLine("Add meg egy verseny helyszínét: ");
                        string userRequestedLocation = Console.ReadLine();
                        Console.WriteLine($"Add meg mely nemzetiségű pilótákat szeretnéd listázni a {userRequestedLocation} versenyen: ");
                        string userRequestedNationality = Console.ReadLine();
                        driverNationalityDisplayer.DisplayDriversByNationality(userRequestedLocation, userRequestedNationality);
                        break;
                    case 6:
                        fiveBest.DisplayBestFiveRacers();
                        break;
                    case 7:
                        Console.WriteLine("Add meg, hogy melyik csapat versenyzőire vagy kiváncsi");
                        string userRequestedTeamName = Console.ReadLine();
                        var podiumCnts = analyzer.GetPodiumCountByTeam(userRequestedTeamName);
                        outputHandler.DisplayPodiumCounts(podiumCnts);
                        break;


                }
            }




            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
            //var newRace = new GrandPrixes();
            //Console.WriteLine("Add meg a rögzíteni kívánt verseny helyszínét: ");
            //newRace.Location = Console.ReadLine();
            //Console.WriteLine("Add meg a verseny dátumát(Ilyen formában: 2023-07-23): ");
            //newRace.Date = DateTime.Parse(Console.ReadLine());
            //string[] podiumNames = new string[3];
            //Console.WriteLine("Add meg az 1. helyezett nevét: ");
            //podiumNames[0] = Console.ReadLine();
            //Console.WriteLine("Add meg az 2. helyezett nevét: ");
            //podiumNames[1] = Console.ReadLine();
            //Console.WriteLine("Add meg az 3. helyezett nevét: ");
            //podiumNames[2] = Console.ReadLine();

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
        public static void DisplayLoadingAnimation(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.Write("Betöltés: ");
            for (int i = 0; i <= 10; i++)
            {
                Console.Write("█");
                Thread.Sleep(300); // Speed of loading
            }
            Console.WriteLine();
            Console.WriteLine("Kész is vagyunk!");
            Console.ResetColor();
            Thread.Sleep(1000);
            Console.Clear();
        }
    }
}
