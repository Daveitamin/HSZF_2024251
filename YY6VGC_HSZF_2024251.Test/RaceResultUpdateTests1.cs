using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace YY6VGC_HSZF_2024251.Test
{
    public interface IDatabase
    {
        void AddNewDriver(Drivers driver);
    }

    public class Drivers
    {
        public string name { get; set; }
        public string team { get; set; }
        public int points { get; set; }
        public string nationality { get; set; }
        public int GrandPrixesId { get; set; }
    }

    public class RaceResultUpdate
    {
        private readonly IDatabase database;

        public RaceResultUpdate(IDatabase db)
        {
            database = db;
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
                    points = points[i],
                    nationality = "Unknown",
                    GrandPrixesId = 1 // A versenyhez tartozó ID (használhatod az aktuális verseny ID-ját)
                };
                database.AddNewDriver(addNewDriver);
            }
        }
    }



    [TestFixture]
    public class RaceResultUpdateTests1
    {
        private RaceResultUpdate raceResultUpdate;
        private FakeDatabase fakeDatabase;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            raceResultUpdate = new RaceResultUpdate(fakeDatabase);
        }

        [Test]
        public void PointsAddDriver_ShouldAddCorrectPointsToDrivers()
        {
            // Arrange
            string[] podiumDrivers = { "Hamilton", "Verstappen", "Leclerc" };
            int[] expectedPoints = { 25, 15, 8 };

            // Act
            raceResultUpdate.PointsAddDriver(podiumDrivers);

            // Assert
            
            for (int i = 0; i < podiumDrivers.Length; i++)
            {
                //Assert.AreEqual(podiumDrivers[i], fakeDatabase.Drivers[i].name);
                //Assert.AreEqual(expectedPoints[i], fakeDatabase.Drivers[i].points);
                Assert.That(fakeDatabase.Drivers[i].name, Is.EqualTo(podiumDrivers[i]));
                Assert.That(fakeDatabase.Drivers[i].points, Is.EqualTo(expectedPoints[i]));
            }
        }

        // FakeDatabase osztály, amely szimulálja az adatbázist
        private class FakeDatabase : IDatabase
        {
            public List<Drivers> Drivers { get; set; } = new List<Drivers>();

            public void AddNewDriver(Drivers driver)
            {
                Drivers.Add(driver);
            }
        }
    }
}
