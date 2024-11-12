using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Model;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public interface IRaceResultUpdate
    {
        //location, team, podium, drivers (name, team, points)
        void CreateRace(GrandPrixes raceName, string[] names);
        void DeleteRace(int raceId);
        void UpdateRace();
        void CreateNewDriver(Drivers driver);
        void DeleteDriver(int id);
        void UpdateCurrentDriverTeam(string driver, string newTeam);
        void UpdateCurrentDriverPoints(string driver, int newPoint);

    }
    public class RaceResultUpdate : IRaceResultUpdate
    {
        private readonly AppDbContext context;

        public RaceResultUpdate(AppDbContext context)
        {
            this.context = context;
        }

        public void CreateNewDriver(Drivers driver)
        {
            context.Add(driver);
            context.SaveChanges();
        }

        public void DeleteDriver(int id)
        {
            var driverToDelete = context.Drivers.FirstOrDefault(x => x.DriverId == id);
            context.Drivers.Remove(driverToDelete);
            context.SaveChanges();

        }

        public void CreateRace(GrandPrixes raceName, string[] names)
        {
            context.Add(raceName);
            UpdateCurrentDriverPoints(names[0], 20);
            UpdateCurrentDriverPoints(names[1], 15);
            UpdateCurrentDriverPoints(names[2], 10);
            context.SaveChanges();
        }

        public void DeleteRace(int raceId)
        {
            var raceToDelete = context.GrandPrixes.FirstOrDefault(x => x.Id == raceId);
            context.GrandPrixes.Remove(raceToDelete);
            context.SaveChanges();
        }

        public void UpdateCurrentDriverTeam(string driver, string newTeam)
        {
            var driverToUpdate = context.Drivers.FirstOrDefault(x => x.name == driver);
            driverToUpdate.team = newTeam;
            context.SaveChanges();
        }
        public void UpdateCurrentDriverPoints(string driver, int newPoint)
        {
            var driverToUpdate = context.Drivers.FirstOrDefault(x => x.name == driver);
            driverToUpdate.points += newPoint;
            context.SaveChanges();
        }
        public void DeleteDriverPoints(string name)
        {
            var driverPointUpdate = context.Drivers.FirstOrDefault(x => x.name == name);
            if (true)
            {

            }
        }

        public void UpdateRace(int id, string[] newNames)
        {
            
        }
    }
}
