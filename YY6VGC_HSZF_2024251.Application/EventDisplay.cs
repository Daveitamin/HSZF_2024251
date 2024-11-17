using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY6VGC_HSZF_2024251.Application
{
    public class EventDisplay
    {
        public void OnDriverBecameChampion(string driverName)
        {
            Console.WriteLine($"Értesítés: {driverName} átlépte a 300 pontot és ezennel világbajnok lett! ##GRATULÁLUNK##");
        }
    }
}
