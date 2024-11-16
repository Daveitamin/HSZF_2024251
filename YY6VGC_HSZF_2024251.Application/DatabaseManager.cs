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
        void CreateRace(GrandPrixes raceName);
        void DeleteRace(int raceId);

        void CreateDriver(Drivers driver);
        void UpdateRace(int id, string[] newNames);
        void DeleteDriver(int id);
        void UpdateCurrentDriverTeam(string driver, string newTeam);
        void UpdateCurrentDriverPoints(string driver, int newPoint);
    }

    public class DatabaseManager : IDatabaseManager
    {
        private readonly IRaceResultUpdate raceResultUpdate;

        public DatabaseManager(IRaceResultUpdate raceResultUpdate)
        {
            this.raceResultUpdate = raceResultUpdate;
        }

        public void CreateDriver(Drivers driver)
        {
            raceResultUpdate.AddNewDriver(driver);
        }

        public void CreateRace(GrandPrixes raceName)
        {
            raceResultUpdate.CreateRace(raceName);
        }

        public void DeleteDriver(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteRace(int raceId)
        {
            raceResultUpdate.DeleteRace(raceId);
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
            raceResultUpdate.UpdateRacePodium(id, newNames);
        }
    }
}
