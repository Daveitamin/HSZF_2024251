using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YY6VGC_HSZF_2024251.Model;

namespace YY6VGC_HSZF_2024251.Persistence.MsSql
{
    public interface IXMLDataProvider
    {
        List<GrandPrixes> GetAllRaces();
    }

    public class XMLDataProvider : IXMLDataProvider
    {
        private readonly AppDbContext context;
        public XMLDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public List<GrandPrixes> GetAllRaces()
        {
            return context.GrandPrixes.ToList();
        }
    }
    
}
