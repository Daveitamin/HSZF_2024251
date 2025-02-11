﻿using System;
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
        void UpdateRacePodium(int raceId, string[] newNames);
        void AddNewDriver(Drivers driver);
        void DeleteDriver(int id);
        void UpdateCurrentDriverTeam(string driver, string newTeam);
        void UpdateCurrentDriverPoints(string driver);
        void AddDriverPoints(string[] namesToAddPoints);

        void PointsAddDriver(string[] podiumDriverNameCollection);
    }
    public class RaceResultUpdate : IRaceResultUpdate
    {
        private readonly AppDbContext context;

        public RaceResultUpdate(AppDbContext context)
        {
            this.context = context;
        }

        public void AddNewDriver(Drivers driver)
        {
            //var selfGrandPrixesId = 80;
            driver.GrandPrixesId = 1;


            context.Drivers.Add(driver);
            context.SaveChanges();
        }

        public void DeleteDriver(int id)
        {
            var driverToDelete = context.Drivers.FirstOrDefault(x => x.DriverId == id);
            if (driverToDelete == null)
            {
                Console.WriteLine("Nincs ilyen ID-val rendelkező versenyző az adatbázisban!");
            }
            else
            {
                context.Drivers.Remove(driverToDelete);
                Console.WriteLine("Sikeres törlés!");
            }
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
            if (raceToDelete == null)
            {
                Console.WriteLine("Nem létezik ilyen ID-vel verseny az adatbázisban!");
            }
            else
            {
                context.GrandPrixes.Remove(raceToDelete);
                Console.WriteLine("Sikeres törlés!");
            }           
            context.SaveChanges();
        }

        public void UpdateCurrentDriverTeam(string driver, string newTeam)
        {
            var driverToUpdate = context.Drivers.FirstOrDefault(x => x.name == driver);
            driverToUpdate.team = newTeam;
            context.SaveChanges();
        }
        public void UpdateCurrentDriverPoints(string driver)
        {
            //driverToUpdate.points += newPoint;
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

        public void UpdateRacePodium(int raceId, string[] newNames)
        {
            var findRaceId = context.GrandPrixes.FirstOrDefault(x => x.Id == raceId);
            string[] oldNames = findRaceId.Podium;
            //DeleteDriverPoints(oldNames);
            findRaceId.Podium = newNames;
            //AddDriverPoints(newNames);
            if (findRaceId == null)
            {
                Console.WriteLine("Nem található ilyen verseny az adatbázisban.");
                return;
            }

            findRaceId.Podium = newNames;
            context.SaveChanges();
            Console.WriteLine($"{findRaceId.Location} helyszínű verseny podiumja frissítve lett.");

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
                var newPilot = new Drivers { name = pilotName, team = "Unknown", points = 0 };
                context.Drivers.Add(newPilot);
                context.SaveChanges();
                Console.WriteLine($"A {newPilot} pilóta létrehozva az adatbázisban");
            }
        }

        public void PointsAddDriver(string[] podiumDriverNameCollection)
        {
            int[] points = new int[] { 25, 15, 8 }; // Pontszámok
            for (int i = 0; i < podiumDriverNameCollection.Length; i++)
            {
                Drivers addNewDriver = new Drivers
                {
                    name = podiumDriverNameCollection[i],
                    team = "Unknown",
                    points = points[i], // A megfelelő pontot rendeljük hozzá
                    nationality = "Unknown",
                    GrandPrixesId = 1 // A versenyhez tartozó ID (használhatod az aktuális verseny ID-ját)
                };
                AddNewDriver(addNewDriver);
            }


        }
    }
}
