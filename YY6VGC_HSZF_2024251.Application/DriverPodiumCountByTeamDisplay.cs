using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Application
{
    public interface IDriverPodiumCountByTeamDisplay
    {
        void DisplayPodiumCounts(List<(string DriverName, int PodiumCount)> podiumCounts);
    }
    public class DriverPodiumCountByTeamDisplay : IDriverPodiumCountByTeamDisplay
    {
        private readonly ITeamDriverAnalyzerDataProvider teamDriverAnalyzerDataProvider;

        public DriverPodiumCountByTeamDisplay(ITeamDriverAnalyzerDataProvider teamDriverAnalyzerDataProvider)
        {
            this.teamDriverAnalyzerDataProvider = teamDriverAnalyzerDataProvider;
        }

        public void DisplayPodiumCounts(List<(string DriverName, int PodiumCount)> podiumCounts)
        {
            Console.WriteLine("\nVersenyzők dobogós helyezései csökkenő sorrendben: ");
            foreach (var item in podiumCounts)
            {
                Console.WriteLine($"\nVersenyző: {item.DriverName} | Dobogós helyezések: {item.PodiumCount}");
            }
        }
    }
}
