using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY6VGC_HSZF_2024251.Application
{
    public interface ITxtWriter
    {
        void WriteSeasonSummary(string filePath, Dictionary<string, int> teamScores, Dictionary<string, int> driverScores);

    }
    public class TxtWriter : ITxtWriter
    {
        public void WriteSeasonSummary(string filePath, Dictionary<string, int> teamScores, Dictionary<string, int> driverScores)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("### Season Summary ###");
                writer.WriteLine("\n### Teams ###");
                foreach (var team in teamScores)
                {
                    writer.WriteLine($"{team.Key}: {team.Value} points");
                }

                writer.WriteLine("\n### Drivers ###");
                foreach (var driver in driverScores)
                {
                    writer.WriteLine($"{driver.Key}: {driver.Value} points");
                }
            }
            Console.WriteLine($"Season summary saved to {filePath}");
        }
    }
}
