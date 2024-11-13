using Microsoft.EntityFrameworkCore;
using YY6VGC_HSZF_2024251.Model;
using System.Text;
using Newtonsoft.Json;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public class AppDbContext : DbContext
    {
        public DbSet<GrandPrixes> GrandPrixes { get; set; }
        public DbSet<Drivers> Drivers { get; set; }

        //
        public DbSet<Drivers> EditableDrivers { get; set; }
        public AppDbContext() { this.Database.EnsureCreated(); SeedDataBase(); }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string str = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=database;Integrated Security=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(str);
            base.OnConfiguring(optionsBuilder);
        }

        private void SeedDataBase()
        {
            string jsonpath = "datajson.json";
            string json = File.ReadAllText(jsonpath, Encoding.UTF8);
            var root = JsonConvert.DeserializeObject<Root>(json);
            if (!GrandPrixes.Any())
            {
                foreach (var gp in root.races) //json fájlban lévő összesítő nevek pl "races"
                {
                    var gpEntity = new GrandPrixes
                    {
                        Location = gp.Location, Date = gp.Date, Podium = gp.Podium, Drivers = gp.Drivers
                    };
                    Console.WriteLine($"Location: {gp.Location}, Date: {gp.Date}, Drivers Count: {gp.Drivers.Count}");

                    GrandPrixes.Add(gpEntity);//addrange lecserélve
                }

                SaveChanges();
            }
        }


        



    }
}
