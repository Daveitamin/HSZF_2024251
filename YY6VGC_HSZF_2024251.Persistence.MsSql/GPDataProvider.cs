using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Model;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public interface IGPDataProvider
    {

        List<string> GetLocations();
    }

    public class GPDataProvider : IGPDataProvider
    {

        private readonly AppDbContext context;
        public GPDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public List<string> GetLocations()
        {
            return context.GrandPrixes.Select(gp => gp.Location).ToList();
        }
    }
}
