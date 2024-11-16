using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Application
{
    public interface IDriverNationalityDisplay
    {
        public void DisplayDriversByNationality(string raceLocation, string requestedNationality);
    }
    public class DriverNationalityDisplay : IDriverNationalityDisplay
    {
        private readonly IDriverByNationalityLister driverNationalityDisplay;

        public DriverNationalityDisplay(IDriverByNationalityLister driverNationalityDisplay)
        {
            this.driverNationalityDisplay = driverNationalityDisplay;
        }

        public void DisplayDriversByNationality(string raceLocation, string requestedNationality)
        {
            var driverNationality = driverNationalityDisplay.GetDriversByNationalityOnRace(raceLocation, requestedNationality);

            Console.WriteLine($"{requestedNationality} versenyzők a {raceLocation} versenyen:");
            foreach (var driver in driverNationality)
            {
                Console.WriteLine(driver.name);
            }
        }
    }
}
