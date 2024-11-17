using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YY6VGC_HSZF_2024251.Application
{
    public static class FileLineCounter
    {
        /// <summary>
        /// Megszámolja az összes sor számát a megadott mappában található fájlokban.
        /// </summary>
        /// <param name="directory">A mappa elérési útvonala.</param>
        /// <param name="searchPattern">A keresési mintázat (alapértelmezés: "*.*").</param>
        /// <returns>A fájlok elérési útvonala és sorainak száma.</returns>
        public static List<FileLineInfo> GetFilesLineCount(string directory, string searchPattern = "*.*")
        {
            if (directory == null || !Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"A megadott mappa nem található: {directory}");
            }

            return Directory.EnumerateFiles(directory, searchPattern, SearchOption.AllDirectories)
                            .Select(file => new FileLineInfo
                            {
                                FilePath = file,
                                TotalLines = File.ReadLines(file).Count()
                            }).ToList();
        }
    }

    public class FileLineInfo
    {
        public string FilePath { get; set; }
        public int TotalLines { get; set; }
    }
}
