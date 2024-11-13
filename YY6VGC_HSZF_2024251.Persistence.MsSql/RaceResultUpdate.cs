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
        void CreateRace(GrandPrixes raceName);
        void DeleteRace(int raceId);
        void UpdateRace(int id, string[] newNames);
        void CreateNewDriver(Drivers driver);
        void DeleteDriver(int id);
        void UpdateCurrentDriverTeam(string driver, string newTeam);
        void UpdateCurrentDriverPoints(string driver, int newPoint);
        void AddDriverPoints(string[] namesToAddPoints);

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

        public void CreateRace(GrandPrixes raceName)
        {
            var existingDataBase = context.GrandPrixes.FirstOrDefault(x => x.Location == raceName.Location);

            if (existingDataBase != null)
            {
                Console.WriteLine($"Ilyen verseny már létezik az adatbázisban ezért nem tudod hozzáadni.");
            }
            else
            {
                context.Add(raceName);
            }



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
        public void DeleteDriverPoints(string[] namesToDeletePoints)
        {
            int i = 0;
            foreach (var item in namesToDeletePoints)
            {
                var driverToDeletePoints = context.Drivers.FirstOrDefault(x => x.name == item);
                if (i == 0)
                {
                    driverToDeletePoints.points -= 20;
                    i++;
                }
                else if (i == 1)
                {
                    driverToDeletePoints.points -= 15;
                    i++;
                }
                else
                {
                    driverToDeletePoints.points -= 10;
                    i++;
                }
            }
        }
        public void AddDriverPoints(string[] namesToAddPoints)
        {
            int i = 0;
            foreach (var item in namesToAddPoints)
            {
                var driverToDeletePoints = context.Drivers.FirstOrDefault(x => x.name == item);
                if (i == 0)
                {
                    driverToDeletePoints.points += 20;
                    i++;
                }
                else if (i == 1)
                {
                    driverToDeletePoints.points += 15;
                    i++;
                }
                else
                {
                    driverToDeletePoints.points += 10;
                    i++;
                }
            }
        }

        public void UpdateRace(int id, string[] newNames)
        {
            var findRaceId = context.GrandPrixes.FirstOrDefault(x => x.Id == id);
            string[] oldNames = findRaceId.Podium;
            DeleteDriverPoints(oldNames);
            findRaceId.Podium = newNames;
            AddDriverPoints(newNames);

        }

        public void CheckOrCreatePilot(string pilotName, string[] teamname)
        {
            var existingPilot = context.Drivers.FirstOrDefault(x => x.name == pilotName);

            if (existingPilot != null)
            {
                Console.WriteLine($"A {pilotName} nevű pilóta már létezik az adatbázisban.");
            }
            else
            {
                var newPilot = new Drivers { name = pilotName, team = "Unknown", points = 0};
                context.Drivers.Add(newPilot);
                context.SaveChanges();
                Console.WriteLine($"A {newPilot} pilóta létrehozva az adatbázisban");
            }
        }



    }
}
