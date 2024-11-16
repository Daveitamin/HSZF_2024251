using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Persistence.MsSql;

namespace YY6VGC_HSZF_2024251.Application
{
    public class CustomEventHandler : Globals
    {
        private readonly AppDbContext context;

        public event EventHandler<string> WinnerPilotFound;

        public CustomEventHandler(AppDbContext context)
        {
            this.context = context;
        }

        public void CheckAndNotifyWinner()
        {
            if (IsEventProvider())
            {
                WinnerFound($"{racerName} elérte a 300 pontot!!! Graulálunk");
            }
        }

        private bool IsEventProvider()
        {
            return context.GrandPrixes.Any(gp => gp.Location == "Monaco");
        }

        protected virtual void WinnerFound(string message)
        {
            WinnerPilotFound?.Invoke(this, message);
        }


    }

}
