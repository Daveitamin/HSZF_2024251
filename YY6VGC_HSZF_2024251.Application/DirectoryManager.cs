using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Application
{
    public interface IDirectoryManager
    {
        public void CreateRaceFolders();
    }
    public class DirectoryManager : IDirectoryManager
    {
        private readonly IGPDataProvider gPDataProvider;
        private string mainFolderPath;

        public DirectoryManager(IGPDataProvider gPDataProvider)
        {
            this.gPDataProvider = gPDataProvider;
            mainFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Formula1_2023");
            if (!Directory.Exists(mainFolderPath))
            {
                Directory.CreateDirectory(mainFolderPath);
                Console.WriteLine("Létrehozva: Formula1_2023 fő mappa");
            }
        }

        public void CreateRaceFolders()
        {
            
            List<string> locations = gPDataProvider.GetLocations();
            foreach (var item in locations)
            {
                string raceFolderPath = Path.Combine(mainFolderPath, item);

                if (!Directory.Exists(raceFolderPath))
                {
                    Directory.CreateDirectory(raceFolderPath);
                    Console.WriteLine($"Létrehozva: {item} almappa létrehozva a fő mappában.");
                }
                else
                {
                    Console.WriteLine($"{item} almappa már létezik a főmappában.");
                }
            }

        }
    }
}
