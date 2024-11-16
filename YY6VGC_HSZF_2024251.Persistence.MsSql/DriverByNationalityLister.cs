using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Model;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public interface IDriverByNationalityLister
    {
        List<Drivers> GetDriversByNationalityOnRace(string raceLocation, string nationality);
    }
    public class DriverByNationalityLister : IDriverByNationalityLister
    {
        private readonly AppDbContext context;

        public DriverByNationalityLister(AppDbContext context)
        {
            this.context = context;
        }

        public List<Drivers> GetDriversByNationalityOnRace(string raceLocation, string nationality)
        {
            var race = context.GrandPrixes.FirstOrDefault(gp => gp.Location == raceLocation);

            if (race == null)
            {
                Console.WriteLine("A verseny nem található.");
                return new List<Drivers>();
            }

            var driversOnRace = context.Drivers.Where(d => d.GrandPrixes.Location == raceLocation && d.nationality == nationality).ToList();

            return driversOnRace;





        }
    }
}
