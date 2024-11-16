using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public interface ITeamDriverAnalyzerDataProvider
    {
        List<(string DriverName, int PodiumCount)> GetPodiumCountByTeam(string teamName);
    }
    public class TeamDriverAnalyzerDataProvider : ITeamDriverAnalyzerDataProvider
    {
        private readonly AppDbContext context;

        public TeamDriverAnalyzerDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public List<(string DriverName, int PodiumCount)> GetPodiumCountByTeam(string teamName)
        {
            var driverNames = context.Drivers.Where(d => d.team == teamName).Select(d => d.name).Distinct().ToList();


            var podiumCounts = context.GrandPrixes
                .Where(gp => gp.Podium != null)
                .ToList()
                .SelectMany(gp => gp.Podium)
                .Where(podiumName => driverNames.Contains(podiumName))
                .GroupBy(driver => driver)
                .Select(group => (DriverName: group.Key, PodiumCount: group.Count()))
                .OrderByDescending(p => p.PodiumCount)
                .ToList();

            return podiumCounts;
        }
    }
}
