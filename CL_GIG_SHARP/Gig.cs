using CL_GIG_SHARP.Utils;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace CL_GIG_SHARP
{
    public class Gig
    {
        public string Site { get; set; }
        public string SiteCode { get; set; }
        public string Url { get; set; }
        public HtmlWeb Web { get; set; }
        public HtmlDocument Document { get; set; }
        public HtmlNodeCollection Nodes { get; set; }
        public Dictionary<string, string> Gigs { get; set; } 

        public Gig(string site)
        {
            // setup web scrape
            SiteCode = site;
            Url = "http://" + site + ".craigslist.org/search/cpg";
            Web = new HtmlWeb();
            Web.UserAgent = Constants.USER_AGENT;
            Document = Web.Load(Url);
            Site = Document.DocumentNode.SelectSingleNode("//head/title").InnerText.ToUpper();
            Gigs = new Dictionary<string, string>();
            
            // search areaAbb for more sites in select 
            if(Program.UpdateSubCities)
                ScrapeSubAreas();
            
            // scrape out href tags and outer html
            Nodes = Document.DocumentNode.SelectNodes("//a[contains(@class, 'hdrlnk')]");
            if (Nodes == null) return;          
            foreach (var node in Nodes)
            {
                if (Gigs.ContainsKey(node.InnerText.ToUpper())) continue;
                Gigs[node.InnerText.ToUpper()] = node.Attributes["href"].Value;
            }
        }

        private void ScrapeSubAreas()
        {
            // scrape out sub area from gig site
            // re-run after running a sub area scrap to update site txt
            var sites = Document.DocumentNode.SelectSingleNode("//select[@id='areaAbb']");
            if (sites == null) return;     
            List<string> areas = sites.Descendants("option")
                                                        .Skip(1)
                                                        .Select(n =>  n.Attributes["value"].Value)
                                                        .ToList();

            List<string> newAreas = new List<string>();
            foreach(var area in areas)
            {
                if(!Program.Cities.Contains(area))
                    newAreas.Add(area);
            }
            if (newAreas.Count > 0) SitesLoader.AppendCities(newAreas);
        }
    }
}
