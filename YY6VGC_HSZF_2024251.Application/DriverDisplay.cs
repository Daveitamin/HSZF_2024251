using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Application
{
    public interface IDriverDisplay
    {
        void DisplayDriversByPoints();
    }
    public class DriverDisplay : IDriverDisplay
    {
        private readonly IDriverLister driverLister;


        public DriverDisplay(IDriverLister driverLister)
        {
            this.driverLister = driverLister;
        }

        public void DisplayDriversByPoints()
        {
            var drivers = driverLister.GetDriversOrderedByPoints();
            Console.WriteLine("Versenyzők listázása pontszám szerint csökkenő sorrendben: ");
            foreach (var driver in drivers)
            {
                Console.WriteLine($"{driver.name} - {driver.team} - {driver.points} pont");
            }
        }
    }
}
