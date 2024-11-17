using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY6VGC_HSZF_2024251.Application
{
    public interface IEventNotifier
    {
        event Action<string> DriverBecameChampion; // Esemény deklarációja

        void CheckForChampion(Dictionary<string, int> driverPoints);
    }

    public class EventNotifier : IEventNotifier
    {
        // Esemény definiálása
        public event Action<string> DriverBecameChampion;

        // Esemény kiváltásának ellenőrzése
        public void CheckForChampion(Dictionary<string, int> driverPoints)
        {
            foreach (var driver in driverPoints)
            {
                if (driver.Value >= 300)
                {
                    // Esemény kiváltása
                    DriverBecameChampion?.Invoke(driver.Key);
                }
            }
        }
    }
}
