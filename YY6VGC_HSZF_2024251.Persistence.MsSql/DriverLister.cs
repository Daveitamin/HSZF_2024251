using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Model;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public interface IDriverLister
    {
        List<Drivers> GetDriversOrderedByPoints();
    }
    public class DriverLister : IDriverLister
    {
        private readonly AppDbContext context;

        public DriverLister(AppDbContext context)
        {
            this.context = context;
        }


        public List<Drivers> GetDriversOrderedByPoints()
        {
            return context.Drivers.GroupBy(d => d.name).Select(g => new Drivers
            {
                name = g.Key,
                team = g.First().team,
                points = g.Sum(d => d.points)
            }).OrderByDescending(d => d.points).ToList();
            //return context.Drivers.OrderByDescending(d => d.points).ToList();
        }
    }
}
