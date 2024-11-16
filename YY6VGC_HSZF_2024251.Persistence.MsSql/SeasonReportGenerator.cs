using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public interface ISeasonReportGenerator
    {
        Dictionary<string, int> GetTeamScores();
        Dictionary<string, int> GetDriverScores();
    }
    public class SeasonReportGenerator : ISeasonReportGenerator
    {
        private readonly AppDbContext context;

        public SeasonReportGenerator(AppDbContext context)
        {
            this.context = context;
        }

        public Dictionary<string, int> GetDriverScores()
        {
            return context.Drivers.GroupBy(d => d.name).ToDictionary(g => g.Key, g => g.Sum(d => d.points));
        }

        public Dictionary<string, int> GetTeamScores()
        {
            return context.Drivers.GroupBy(d => d.team).ToDictionary(g => g.Key, g => g.Sum(d => d.points));
        }
    }
}
