using CL_GIG_SHARP.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CL_GIG_SHARP
{
    class Program
    {
        public static bool UpdateSubCities = false;
        public static List<string> Cities { get; set; }
        static List<Gig> Gigs { get; set; } = new List<Gig>();
        static int GigCount { get; set; }



        static void Main(string[] args)
        {
            

            // load city text file
            Cities = SitesLoader.LoadCities();

            // scrape cities for gigs
            foreach (var site in Cities)
            {
                Console.WriteLine($"[*] crawling {site}");
                Gig gcg = new Gig(site);
                Console.WriteLine($"[*] crawled {gcg.Site} @ {gcg.Gigs.Count} gigs");
                Gigs.Add(gcg);
                GigCount += gcg.Gigs.Count;
                Console.WriteLine($"[*] {Gigs.Count} cities crawled @ {GigCount} gigs");
                Thread.Sleep(1000);

            }

            CreateHTML();
            
        }

        public static void CreateHTML()
        {
            List<string> htmlDoc = new List<string>();
            htmlDoc.Add(Constants.HTML_HEADER);

            int _gigCount = 0;
            foreach(var gig in Gigs)
            {
                foreach(var item in gig.Gigs)
                {
                    ++_gigCount;
                    htmlDoc.Add($"<tr><td>{_gigCount}</td><td>{gig.Site}</td><td><a href=\"{item.Value}\" target=\"_blank\"><b>{item.Key}</b></a></td></tr>");
                }
            }

            htmlDoc.Add(Constants.HTML_FOOTER);
            File.WriteAllLines("temp_craiglist_gig_list.html", htmlDoc.ToArray());
            Console.WriteLine($"[*] html created. view here...");
        }
    }
}
