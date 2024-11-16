using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    //test globref
    public class Globals
    {
        public string racerName { get; private set; } = "Max Verstappen";
    }
    public interface IEventHandlerDataProvider
    {
        event EventHandler<EventHandlerDataProvider> DataChanged;
        public bool IsEventProvider();
    }
    public class EventHandlerDataProvider : IEventHandlerDataProvider
    {

        private readonly AppDbContext context;

        public EventHandlerDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public event EventHandler<EventHandlerDataProvider> DataChanged;

        public bool IsEventProvider()
        {
            return context.GrandPrixes.Any(gp => gp.Location == "Monaco");
        }
    }
}
