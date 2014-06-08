using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scraping.Scrapers
{
    public interface IWebScraper
    {
        string Scrape(string url, string username, string password, int pin);
    }
}
