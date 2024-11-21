using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using YY6VGC_HSZF_2024251.Application;
using YY6VGC_HSZF_2024251.Model;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Test
{
    [TestFixture]
    public class Test
    {
        // Teszt: Verseny létrehozása, amikor nem létezik
        [Test]
        public void CreateRace_ShouldAddRace_WhenRaceDoesNotExist()
        {
            // Arrange
            var mockRaceResultUpdate = new Mock<IRaceResultUpdate>();
            var databaseManager = new DatabaseManager(mockRaceResultUpdate.Object);

            var newRace = new GrandPrixes
            {
                Location = "Monaco",
                Date = DateTime.Parse("2023-05-20"),
                Podium = new string[] { "Driver 1", "Driver 2", "Driver 3" }
            };

            databaseManager.CreateRace(newRace);

            // Assert
            mockRaceResultUpdate.Verify(r => r.CreateRace(It.Is<GrandPrixes>(r => r.Location == "Monaco")), Times.Once);
        }

        // Teszt: Verseny létrehozása, amikor már létezik
        [Test]
        public void CreateRace_ShouldNotAddRace_WhenRaceExists()
        {
            // Arrange
            var mockRaceResultUpdate = new Mock<IRaceResultUpdate>();
            var databaseManager = new DatabaseManager(mockRaceResultUpdate.Object);

            var newRace = new GrandPrixes
            {
                Location = "Monaco",
                Date = DateTime.Parse("2023-05-20"),
                Podium = new string[] { "Driver 1", "Driver 2", "Driver 3" }
            };

            // Szimulalas
            mockRaceResultUpdate.Setup(r => r.CreateRace(It.IsAny<GrandPrixes>()))
                                .Throws(new InvalidOperationException("Race already exists"));

            Assert.Throws<InvalidOperationException>(() => databaseManager.CreateRace(newRace));
        }

        // Teszt: Dátum formátum ellenőrzése
        [Test]
        public void CreateRace_ShouldParseDate_WhenValidDateIsProvided()
        {
            // Arrange
            var validDate = "2023-07-23";
            var expectedDate = DateTime.Parse(validDate);

            DateTime parsedDate;
            var result = DateTime.TryParseExact(validDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsedDate);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(parsedDate, Is.EqualTo(expectedDate));
        }

        // Teszt: Dátum hibás formátum kezelés
        [Test]
        public void CreateRace_ShouldFailToParseDate_WhenInvalidDateIsProvided()
        {
            // Arrange
            var invalidDate = "2023-07-32"; // Érvénytelen dátum

            DateTime parsedDate;
            var result = DateTime.TryParseExact(invalidDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsedDate);

            // Assert
            Assert.That(result, Is.False);
        }

        // Teszt: Pódium versenyzők neveinek hozzáadása
        [Test]
        public void AddPoints_ShouldCallAddPoints_WhenValidPodiumNamesAreGiven()
        {
            // Arrange
            var mockRaceResultUpdate = new Mock<IRaceResultUpdate>();
            var databaseManager = new DatabaseManager(mockRaceResultUpdate.Object);

            var podiumNames = new string[] { "Driver 1", "Driver 2", "Driver 3" };

            databaseManager.AddPoints(podiumNames);

            // Assert
            mockRaceResultUpdate.Verify(r => r.PointsAddDriver(It.Is<string[]>(p => p.Length == 3 && p[0] == "Driver 1")), Times.Once);
        }
    }
}
