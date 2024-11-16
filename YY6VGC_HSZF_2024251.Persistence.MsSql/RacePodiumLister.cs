using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public interface IRacePodiumLister
    {
        List<string> GetPodiumbyRaceLocation(string location);
    }
    public class RacePodiumLister : IRacePodiumLister
    {
        private readonly AppDbContext context;

        public RacePodiumLister(AppDbContext context)
        {
            this.context = context;
        }



        public List<string> GetPodiumbyRaceLocation(string location)
        {
            var race = context.GrandPrixes.FirstOrDefault(gp => gp.Location == location);
            if (race == null)
            {
                return new List<string> { "Verseny nem található." };
            }
            var podium = race.Podium.ToList();

            if (podium == null || podium.Count == 0)
            {
                return new List<string> { "Nincs dobogós adat." };
            }
            return podium;
        }
    }
}
