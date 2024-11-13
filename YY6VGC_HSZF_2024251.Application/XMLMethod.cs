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

                XmlSerializer serializer = new XmlSerializer(typeof(GrandPrixes)); ///xmlmethod
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, race);
                }

                Console.WriteLine($"Az eredmények sikeresen exportálva: {filePath}");


            }




        }
    }
}
