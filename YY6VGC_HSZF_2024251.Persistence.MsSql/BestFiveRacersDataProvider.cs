using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Model;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public interface IBestFiveRacersDataProvider
    {
        List<Drivers> GetFiveBestWinnerRacers();
    }
    public class BestFiveRacersDataProvider : IBestFiveRacersDataProvider
    {
        private readonly AppDbContext context;

        public BestFiveRacersDataProvider(AppDbContext context)
        {
            this.context = context;
        }


        public List<Drivers> GetFiveBestWinnerRacers()
        {
            return context.Drivers.Where(d => d.points == 25).GroupBy(d => d.name).Select(g => new Drivers
            {
                name = g.Key,
                team = g.First().name,
                points = g.Sum(d => d.points)
            }).OrderByDescending(d => d.points).ToList();
        }
    }
}
