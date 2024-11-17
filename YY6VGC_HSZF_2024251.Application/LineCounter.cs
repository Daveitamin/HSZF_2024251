using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY6VGC_HSZF_2024251.Application
{
    public class LineCounter
    {
        public int AllLines(string solutionPath)
        {
            if (!Directory.Exists(solutionPath))
            {
                throw new DirectoryNotFoundException($"A megadott mappa nem található {solutionPath}");
            }

            var csFiles = Directory.GetFiles(solutionPath, "*.cs", SearchOption.AllDirectories);
            int totalLines = 0;

            foreach (var file in csFiles)
            {
                var lines = File.ReadAllLines(file);
                totalLines += lines.Length;
            }

            return totalLines;


        }
    }


}
