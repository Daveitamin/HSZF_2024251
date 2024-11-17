using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Application
{
    public interface IBestFiveRacersDisplay
    {
        void DisplayBestFiveRacers();
    }
    public class BestFiveRacersDisplay : IBestFiveRacersDisplay
    {
        private readonly IBestFiveRacersDataProvider bestFiveDataProvider;
        public BestFiveRacersDisplay(IBestFiveRacersDataProvider bestFiveDataProvider)
        {
            this.bestFiveDataProvider = bestFiveDataProvider;
        }


        public void DisplayBestFiveRacers()
        {
            var bestFiveDrivers = bestFiveDataProvider.GetFiveBestWinnerRacers();
            //Console.WriteLine("A legjobb 5 versenyző akik legalább 2 versenyen győzedelmeskedtek: ");
            int i = 1;
            foreach (var driver in bestFiveDrivers)
            {
                Console.WriteLine($"{i}: {driver.name}");
                i++;
            }
        }
    }
}
