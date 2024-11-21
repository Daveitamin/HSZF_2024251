using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using YY6VGC_HSZF_2024251.Application;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Test
{
    [TestFixture]
    public class Test3
    {
        // Teszt: Dobogósok listázása helyes helyszín alapján
        [Test]
        public void ShodPodium_ShouldDisplayPodium_WhenRaceLocationExists()
        {
            // Arrange
            var mockRacePodiumLister = new Mock<IRacePodiumLister>();
            var location = "Monaco";
            var expectedPodium = new List<string> { "Driver 1", "Driver 2", "Driver 3" };

            //mock
            mockRacePodiumLister.Setup(r => r.GetPodiumbyRaceLocation(location)).Returns(expectedPodium);

            var podiumDisplay = new PodiumDisplay(mockRacePodiumLister.Object);
            using (var consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);
                podiumDisplay.ShodPodium(location);
                var output = consoleOutput.ToString();

                // Podiumos emberek listazasa
                Assert.That(output, Contains.Substring("Dobogósok a(z) Monaco versenyen:"));
                foreach (var driver in expectedPodium)
                {
                    Assert.That(output, Contains.Substring(driver));
                }
            }
        }

        // Teszt: Ha nem található verseny az adott helyszínen
        [Test]
        public void ShodPodium_ShouldDisplayErrorMessage_WhenRaceNotFound()
        {
            // Arrange
            var mockRacePodiumLister = new Mock<IRacePodiumLister>();
            var location = "Paris"; // Helyszín, amely nem létezik a mock adatban
            var expectedMessage = "Verseny nem található.";

            // mock
            mockRacePodiumLister.Setup(r => r.GetPodiumbyRaceLocation(location)).Returns(new List<string> { expectedMessage });

            var podiumDisplay = new PodiumDisplay(mockRacePodiumLister.Object);

            
            using (var consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);
                podiumDisplay.ShodPodium(location);
                var output = consoleOutput.ToString();

                Assert.That(output, Contains.Substring(expectedMessage));
            }
        }

        // Teszt: Ha nincs dobogós adat az adott versenyhez
        [Test]
        public void ShodPodium_ShouldDisplayNoPodiumMessage_WhenNoPodiumAvailable()
        {
            // Arrange
            var mockRacePodiumLister = new Mock<IRacePodiumLister>();
            var location = "Silverstone";
            var expectedMessage = "Nincs dobogós adat.";

            // mock
            mockRacePodiumLister.Setup(r => r.GetPodiumbyRaceLocation(location)).Returns(new List<string> { expectedMessage });

            var podiumDisplay = new PodiumDisplay(mockRacePodiumLister.Object);

            using (var consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);
                podiumDisplay.ShodPodium(location);
                var output = consoleOutput.ToString();

                // Assert that the output contains the message that no podium data is available
                Assert.That(output, Contains.Substring(expectedMessage));
            }
        }
    }
}
