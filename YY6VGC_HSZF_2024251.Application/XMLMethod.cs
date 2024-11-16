using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YY6VGC_HSZF_2024251.Model;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Application
{
    public interface IXMLMethod
    {
        void SaveAllRacesToXml();
    }
    public class XMLMethod : IXMLMethod
    {
        private readonly IXMLDataProvider xmlDataProvider;

        public XMLMethod(IXMLDataProvider xmlDataProvider)
        {
            this.xmlDataProvider = xmlDataProvider;
        }
        public void SaveAllRacesToXml()
        {
            List<GrandPrixes> races = xmlDataProvider.GetAllRaces();

            foreach (var race in races)
            {
                string folderPath = Path.Combine("Formula1_2023", race.Location);

                string fileName = $"results_{race.Location}.xml";
                string filePath = Path.Combine(folderPath, fileName);

                //xml seralizernek megfelelő formátumba átalakítás különben --> hibát dob és nem fut le a kód


                var raceWithPodium = new GrandPrixesWithPodium(race);


                XmlSerializer serializer = new XmlSerializer(typeof(GrandPrixesWithPodium)); ///xmlmethod
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, raceWithPodium);
                }

                Console.WriteLine($"Az eredmények sikeresen exportálva: {filePath}");


            }

        }


        public class GrandPrixesWithPodium : GrandPrixes
        {
            public GrandPrixesWithPodium() { }
            public GrandPrixesWithPodium(GrandPrixes original)
            {
                Location = original.Location;
                Date = original.Date;
                PodiumList = original.Podium?.ToList() ?? new List<string>();
            }

            [XmlArray("Podium")]
            [XmlArrayItem("Driver")]
            public List<string> PodiumList { get; set; } = new List<string>();
        }


    }
}
