using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.Eventing.Reader;
using YY6VGC_HSZF_2024251.Application;
using YY6VGC_HSZF_2024251.Model;
using YY6VGC_HSZF_2024251.Persistence.MsSql;
using System.Threading;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;

//Moving Characters
static class TextUtilities
{
    public static void Type(string p_input, int p_delay)
    {
        char[] letters = p_input.ToCharArray();
        foreach (char c in letters)
        {
            Console.Write(c);
            Thread.Sleep(p_delay);
        }
        //Console.Write("\n");
    }
}

namespace YY6VGC_HSZF_2024251
{
    public class Program
    {
        
        static void Main(string[] args)
        {

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

            //winnerNotifier.CheckAndNotifyWinner();

            var dataProvider = new EventHandlerDataProvider(apdv);
            var eventNotifier = new EventNotifier();
            var eventDisplay = new EventDisplay();

            eventNotifier.DriverBecameChampion += eventDisplay.OnDriverBecameChampion;

            //txtiras



            IDatabaseManager databaseManager = host.Services.GetService<IDatabaseManager>();



            ////////////GUI////////////////
            ///betöltés
            DisplayLoadingAnimation("Kurva eget");

            
            

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(Logos.dataCenterLogo);
            Console.ForegroundColor = ConsoleColor.Green;
            TextUtilities.Type("Formula 1 Data Center", 30);
            TextUtilities.Type("\nA folytatáshoz nyomj ENTER-t!", 10);
            Console.ReadKey();
            bool firstLogoEntry = true;
            bool exit = false;
            while (!exit)
            {
                if (!firstLogoEntry)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(Logos.dataCenterLogo);
                }
                firstLogoEntry = false;
                int runningConfig = 0;
                //Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\nMENU:");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("1. Új verseny létrehozása");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("2. Új pilóta létrehozása");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("3. Meglévő pilóta törlése");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("4. Meglévő verseny módosítása");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("5. Meglévő verseny törlése ID alapján");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("6. Verseny adatainak exportálása XML formátumban");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("7. Összesített csapat és versenyzői pontszámok exportálása txt formátumban");
                Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine("################################################################");
                Console.WriteLine("Adatbázis lekérdezések: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("8. Versenyzők listázása pontszám szerint csökkenő sorrendben");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("9. Egy adott verseny dobogósainak megjelenítése");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("10. A futamokon résztvevő versenyzők szűrése nemzetiség szerint");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("11. A legjobb 5 versenyző akik legalább 2 különböző versenyen nyertek");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("12. Egy adott csapat versenyzőinek dobogós helyezései");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("13. EXIT");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("A választott menüpont: ");
                runningConfig = int.Parse(Console.ReadLine());
                Console.ForegroundColor = ConsoleColor.White;
                switch (runningConfig)
                {
                        ///Új verseny létrehozása majd feltöltése az adatbázisba------------------------------------------------
                    case 1:
                        Console.Clear();
                        TextUtilities.Type("Új verseny létrehozása:", 20);
                        var newRace = new GrandPrixes();
                        Console.Write("\n\nAdd meg a verseny helyszínét: ");
                        newRace.Location = Console.ReadLine();
                        TextUtilities.Type("\nAdd meg a verseny dátumát(Ilyen formában: 2023-07-23): ",10);
                        newRace.Date = DateTime.Parse(Console.ReadLine());
                        Console.WriteLine("\nAdd meg a pódiumon helyt foglaló versenyzőket: ");

                        string[] podiumNames = new string[3];
                        TextUtilities.Type("1. helyezett neve:", 10);
                        podiumNames[0] = Console.ReadLine();
                        TextUtilities.Type("2. helyezett neve:", 10);
                        podiumNames[1] = Console.ReadLine();
                        TextUtilities.Type("3. helyezett neve:", 10);
                        podiumNames[2] = Console.ReadLine();
                        newRace.Podium = podiumNames;
                        databaseManager.AddPoints(podiumNames);
                        databaseManager.CreateRace(newRace);
                        //Világbajnok event notification vizsgálat ide
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        var championPoints = dataProvider.GetAggregatedDriverPoints();
                        eventNotifier.CheckForChampion(championPoints);
                        Console.ForegroundColor= ConsoleColor.White;
                        Console.WriteLine("Nyomj ENTER-t a folytatáshoz!");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                        //Új versenyző létrehozása------------------------------------------------------------------------------
                    case 2:
                        Console.Clear();
                        TextUtilities.Type("Új versenyző létrehozása", 30);
                        Console.Write("\n\nAdd meg a versenyző nevét: ");
                        string addDriverName = Console.ReadLine();
                        TextUtilities.Type("\nAdd meg a csapat nevét: ", 10);
                        string addDriverTeam = Console.ReadLine();
                        Console.Write("\nAdd meg a versenyző nemzetiségét: ");
                        string addDriverNationality = Console.ReadLine();

                        int? grandPrixesId = null;

                        Drivers newDriver = new Drivers
                        {
                            name = addDriverName,
                            team = addDriverTeam,
                            nationality = addDriverNationality,
                            GrandPrixesId = grandPrixesId

                        };

                        databaseManager.CreateDriver(newDriver);
                        TextUtilities.Type("Versenyző sikeresen hozzáadva!", 20);
                        Thread.Sleep(3000);
                        Console.Clear();

                        break;
                        //Meglévő pilóta törlése felhasználó által megadott ID-val----------------------------------------------
                    case 3:
                        Console.Clear();
                        TextUtilities.Type("Meglévő pilóta törlése", 30);
                        Console.Write("\n\nAdd meg a törölni kívánt pilóta id-ját: ");
                        int deleteId = int.Parse(Console.ReadLine());
                        databaseManager.DeleteDriver(deleteId);
                        Thread.Sleep(2300);
                        break;
                        //Meglévő verseny módosítása----------------------------------------------------------------------------
                    case 4:
                        Console.Clear();
                        TextUtilities.Type("Versenymódosítás", 40);
                        Console.Write("\n\nAdd meg a módosítani kívánt verseny id-ját: ");
                        int userRequestedRaceId = int.Parse(Console.ReadLine());
                        string[] userModifyNames = new string[3];
                        TextUtilities.Type("\n1. helyezett neve:", 10);
                        userModifyNames[0] = Console.ReadLine();
                        TextUtilities.Type("2. helyezett neve:", 10);
                        userModifyNames[1] = Console.ReadLine();
                        TextUtilities.Type("3. helyezett neve:", 10);
                        userModifyNames[2] = Console.ReadLine();
                        databaseManager.UpdateRace(userRequestedRaceId, userModifyNames);
                        Thread.Sleep(3000);
                        break;
                        //Meglévő verseny törlése ID alapján
                    case 5:
                        Console.Clear();
                        TextUtilities.Type("Verseny törlése", 30);
                        Console.Write("\n\nAdd meg a verseny ID-ját amit törölni szeretnél: ");
                        int versenyId = int.Parse(Console.ReadLine());
                        databaseManager.DeleteRace(versenyId);
                        Thread.Sleep(2300);
                        break;
                        //Versenyek adatai XML formátumú mentése------------------------------------------------------------------
                    case 6:
                        Console.Clear();
                        TextUtilities.Type("Verseny adatainak XML formátumú mentése", 20);
                        xmlll.SaveAllRacesToXml();
                        Thread.Sleep(2600);
                        break;
                        //Riport készítése -- Összetett csapat és versenyzői pontszámok exportálása (SeasonSummary.txt)----------
                    case 7:
                        Console.Clear();
                        TextUtilities.Type("Összetett csapat és versenyzői pontszámok exportálása", 20);
                        Console.WriteLine("\n");
                        Dictionary<string, int> teamsScores = summaryProvider.GetTeamScores();
                        Dictionary<string, int> driverScores = summaryProvider.GetDriverScores();
                        string filePath = Path.Combine("Formula1_2023", "SeasonSummary.txt");
                        txtWriter.WriteSeasonSummary(filePath, teamsScores, driverScores);
                        Thread.Sleep(3000);
                        break;
                        //Versenyzők listázása pontszám szerinti csökkenő sorrendben-----------------------------------------------
                    case 8:
                        Console.Clear();
                        TextUtilities.Type("Versenyzők pontszám alapján csökkenő sorrendben", 20);
                        Console.WriteLine("\n");
                        driverDiplay.DisplayDriversByPoints();
                        Console.WriteLine("\nNyomj ENTER-t a folytatáshoz!");
                        Console.ReadKey();
                        Console.Clear();
                        
                        break;
                        //Egy adott verseny dobogósainak megjelenítése
                    case 9:
                        Console.Clear();
                        TextUtilities.Type("Verseny dobogósainak megjelenítése", 20);
                        Console.Write("\n\nAdd meg a verseny helyszínét: ");
                        string requestLocation = Console.ReadLine();
                        podiumDisplay.ShodPodium(requestLocation);
                        Console.WriteLine("\nNyomj ENTER-t a folytatáshoz!");
                        Console.ReadKey();
                        break;
                        //A futamokon résztvevő versenyzők szűrése nemzetiség szerint
                    case 10:
                        Console.Clear();
                        TextUtilities.Type("Versenyzők szűrése nemzetiség szerint", 20);
                        Console.Write("\n\nAdd meg egy verseny helyszínét: ");
                        string userRequestedLocation = Console.ReadLine();
                        Console.Write($"\nAdd meg mely nemzetiségű pilótákat szeretnéd listázni a {userRequestedLocation} versenyen: ");
                        string userRequestedNationality = Console.ReadLine();
                        driverNationalityDisplayer.DisplayDriversByNationality(userRequestedLocation, userRequestedNationality);
                        Console.WriteLine("\nNyomj ENTER-t a folytatáshoz!");
                        Console.ReadKey();
                        break;
                        // A legjobb 5 versenyző akik legalább 2 különböző versenyen nyertek
                    case 11:
                        Console.Clear();
                        TextUtilities.Type("Legjobb 5 versenyző akik legalább 2 különböző versenyen nyertek: ",10);
                        Console.WriteLine("\n");
                        fiveBest.DisplayBestFiveRacers();
                        Console.WriteLine("\nNyomj ENTER-t a folytatáshoz!");
                        Console.ReadKey();
                        break;
                        //Adott csapat versenyzőinek dobogós helyezései
                    case 12:
                        Console.Clear();
                        TextUtilities.Type("Adott csapat versenyzőinek dobogós helyezései: ", 20);
                        Console.Write("\n\nAdd meg, hogy melyik csapat versenyzőire vagy kiváncsi: ");
                        string userRequestedTeamName = Console.ReadLine();
                        var podiumCnts = analyzer.GetPodiumCountByTeam(userRequestedTeamName);
                        outputHandler.DisplayPodiumCounts(podiumCnts);
                        Console.WriteLine("\nNyomj ENTER-t a folytatáshoz!");
                        Console.ReadKey();
                        break;
                    case 13:
                        exit = true;
                        break;
                    case 0:
                        Console.Clear();
                        Console.Write("Created by:\n");
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        TextUtilities.Type(Logos.creatorName, 10);

                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        try
                        {
                            // Adjuk meg a megvizsgálandó mappa elérési útját
                            string currentDirectory = Directory.GetCurrentDirectory();
                            string? solutionDirectory = GetSolutionDirectory(currentDirectory);

                            if (solutionDirectory == null)
                            {
                                Console.WriteLine("Nem sikerült megtalálni a Solution gyökérmappáját.");
                                return;
                            }

                            // Fájlok és sorszámok lekérése
                            var fileLineCounts = FileLineCounter.GetFilesLineCount(solutionDirectory, "*.cs");

                            // Eredmények kiírása
                            Console.WriteLine($"A solution mappában található .cs fájlok sora:");
                            foreach (var fileInfo in fileLineCounts)
                            {
                                //Console.WriteLine($"{fileInfo.FilePath}: {fileInfo.TotalLines} sor");
                            }

                            Console.WriteLine($"Összesen {fileLineCounts.Sum(info => info.TotalLines)} sor kód található.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Hiba történt: {ex.Message}");
                        }

                        static string? GetSolutionDirectory(string startDirectory)
                        {
                            var directory = new DirectoryInfo(startDirectory);

                            while (directory != null)
                            {
                                if (Directory.GetFiles(directory.FullName, "*.sln").Length > 0)
                                {
                                    return directory.FullName;
                                }

                                directory = directory.Parent;
                            }

                            return null;
                        }
                        Console.WriteLine("Folytatáshoz, nyomj ENTER-t!");
                        Console.ReadKey();
                        Console.BackgroundColor= ConsoleColor.Black;
                        break;
                }
            }



            


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Új pilóta felvétele
            //var driver = new Drivers();
            //Console.WriteLine("Add meg egy sofőr nevét:");
            //driver.name = Console.ReadLine();
            //Console.WriteLine($"Add meg {driver.name} csapatát: ");
            //driver.team = Console.ReadLine();
            //Console.WriteLine($"Add meg, hogy {driver.name}-nek hány pontja van az idei szezonban: ");
            //driver.points = int.Parse(Console.ReadLine());


            //Meglévő pilóta törlése

            //Meglévő pilóta csapata vagy pontja változtatása
            //string driverEditName;
            //Console.WriteLine("Add meg a pilóta nevét(HELYESEN!), akinek a csapatát vagy pontját szeretnéd változtatni.");
            //driverEditName = Console.ReadLine();
            //Console.WriteLine($"Add meg {driverEditName} új csapatát: ");
            //string driverEditNewTeam = Console.ReadLine();
            //Console.WriteLine($"Add meg {driverEditName} szerzett pontjait: ");
            //int driverEditPointIncrease = int.Parse(Console.ReadLine());


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
            //Console.WriteLine("Add meg a törölni kívánt verseny id-ját: ");
            //int deleteRaceId = int.Parse(Console.ReadLine());

            ////Meglévő verseny eredményének módosítása
            //Console.WriteLine("Add meg a módosítani kívánt verseny id-ját");
            //int modifyRaceId = int.Parse(Console.ReadLine());
            //string[] modifyRacePodiumNames = new string[3];
            //Console.WriteLine("Kérlek add meg az új 1. helyezettet:");
            //modifyRacePodiumNames[0] = Console.ReadLine();
            //Console.WriteLine("Kérlek add meg az új 2. helyezettet:");
            //modifyRacePodiumNames[1] = Console.ReadLine();
            //Console.WriteLine("Kérlek add meg az új 3. helyezettet:");
            //modifyRacePodiumNames[2] = Console.ReadLine();


            ///linecounter
            

            
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
