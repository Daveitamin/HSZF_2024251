using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public interface IEventHanderDataProvider
    {
        Dictionary<string, int> GetAggregatedDriverPoints();
    }
    //test globref
    public class EventHandlerDataProvider : IEventHanderDataProvider
    {
        private readonly AppDbContext context;

        public EventHandlerDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public Dictionary<string, int> GetAggregatedDriverPoints()
        {
            return context.Drivers.GroupBy(d => d.name).ToDictionary(group => group.Key, group => group.Sum(d => d.points));
        }
    }
}
