using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Model;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Application
{
    public interface IDatabaseManager
    {
        void CreateRace(GrandPrixes raceName, string[] names);
        void DeleteRace(int raceId);
        void UpdateRace(int id, string[] newNames);
        void CreateNewDriver(Drivers driver);
        void DeleteDriver(int id);
        void UpdateCurrentDriverTeam(string driver, string newTeam);
        void UpdateCurrentDriverPoints(string driver, int newPoint);
        void AddDriverPoints(string[] namesToAddPoints);
    }

    public class DatabaseManager : IDatabaseManager
    {
        private readonly IRaceResultUpdate raceResultUpdate;

        public DatabaseManager(IRaceResultUpdate raceResultUpdate)
        {
            this.raceResultUpdate = raceResultUpdate;
        }

        public void AddDriverPoints(string[] namesToAddPoints)
        {
            throw new NotImplementedException();
        }

        public void CreateNewDriver(Drivers driver)
        {
            throw new NotImplementedException();
        }

        public void CreateRace(GrandPrixes raceName, string[] names)
        {
            raceResultUpdate.CreateRace(raceName, names);
        }

        public void DeleteDriver(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteRace(int raceId)
        {
            throw new NotImplementedException();
        }

        public void UpdateCurrentDriverPoints(string driver, int newPoint)
        {
            throw new NotImplementedException();
        }

        public void UpdateCurrentDriverTeam(string driver, string newTeam)
        {
            throw new NotImplementedException();
        }

        public void UpdateRace(int id, string[] newNames)
        {
            throw new NotImplementedException();
        }
    }
}
