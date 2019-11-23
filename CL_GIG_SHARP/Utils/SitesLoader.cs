using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CL_GIG_SHARP.Utils
{
    public static class SitesLoader
    {
        public static List<string> LoadCities()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Utils\sites.txt");
            var files = File.ReadAllLines(path);
            return new List<string>(files);
        }

        public static void AppendCities(List<string> cities)
        {
            int sitesAdded = 0;
            foreach(var city in cities)
            {
                File.AppendAllText(Constants.SITES_FILE, city + Environment.NewLine);
                ++sitesAdded;
            }
            Console.WriteLine($"[*] added {sitesAdded} new sites to sites file");
        }
    }
}
