using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Application
{
    public interface IPodiumDisplay
    {
        void ShodPodium(string location);
    }
    public class PodiumDisplay : IPodiumDisplay
    {
        private readonly IRacePodiumLister podiumLister;

        public PodiumDisplay(IRacePodiumLister podiumLister)
        {
            this.podiumLister = podiumLister;
        }

        public void ShodPodium(string location)
        {
            var podium = podiumLister.GetPodiumbyRaceLocation(location);

            Console.WriteLine($"Dobogósok a(z) {location} versenyen: ");
            foreach (var driver in podium)
            {
                Console.WriteLine(driver);
            }
        }
    }
}
