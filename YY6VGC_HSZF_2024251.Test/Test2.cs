using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using YY6VGC_HSZF_2024251.Application;
using YY6VGC_HSZF_2024251.Model;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Test
{
    [TestFixture]
    public class Test2
    {
        // Teszt: Verseny podiumjának módosítása, amikor a verseny létezik
        [Test]
        public void UpdateRace_ShouldUpdatePodium_WhenRaceExists()
        {
            // Arrange
            var mockRaceResultUpdate = new Mock<IRaceResultUpdate>();
            var databaseManager = new DatabaseManager(mockRaceResultUpdate.Object);

            var raceId = 1;
            var newPodiumNames = new string[] { "Driver 1", "Driver 2", "Driver 3" };

            var existingRace = new GrandPrixes
            {
                Id = raceId,
                Location = "Monaco",
                Podium = new string[] { "Driver A", "Driver B", "Driver C" }
            };

            mockRaceResultUpdate.Setup(r => r.UpdateRacePodium(raceId, newPodiumNames)).Callback(() =>
            {
                existingRace.Podium = newPodiumNames;
            });

            databaseManager.UpdateRace(raceId, newPodiumNames);

            // Assert
            mockRaceResultUpdate.Verify(r => r.UpdateRacePodium(raceId, newPodiumNames), Times.Once);
            Assert.That(existingRace.Podium, Is.EqualTo(newPodiumNames));
        }

        // Teszt: Verseny podiumjának módosítása, amikor a verseny nem létezik
        [Test]
        public void UpdateRace_ShouldNotUpdatePodium_WhenRaceDoesNotExist()
        {
            // Arrange
            var mockRaceResultUpdate = new Mock<IRaceResultUpdate>();
            var databaseManager = new DatabaseManager(mockRaceResultUpdate.Object);

            var raceId = 999; // Nem létező verseny ID
            var newPodiumNames = new string[] { "Driver 1", "Driver 2", "Driver 3" };

            mockRaceResultUpdate.Setup(r => r.UpdateRacePodium(raceId, newPodiumNames)).Throws(new InvalidOperationException("Nem található ilyen verseny az adatbázisban."));

            var ex = Assert.Throws<InvalidOperationException>(() => databaseManager.UpdateRace(raceId, newPodiumNames));
            Assert.That(ex.Message, Is.EqualTo("Nem található ilyen verseny az adatbázisban."));
        }

        // Teszt: Verseny podiumjának módosítása, ha az új podium érvényes
        [Test]
        public void UpdateRace_ShouldUpdatePodium_WhenValidPodiumNamesAreProvided()
        {
            // Arrange
            var mockRaceResultUpdate = new Mock<IRaceResultUpdate>();
            var databaseManager = new DatabaseManager(mockRaceResultUpdate.Object);

            var raceId = 1;
            var newPodiumNames = new string[] { "Driver 1", "Driver 2", "Driver 3" };

            // Szimulalas
            mockRaceResultUpdate.Setup(r => r.UpdateRacePodium(raceId, newPodiumNames)).Verifiable();

            databaseManager.UpdateRace(raceId, newPodiumNames);

            // Assert
            mockRaceResultUpdate.Verify(r => r.UpdateRacePodium(raceId, newPodiumNames), Times.Once);
        }
    }
}
