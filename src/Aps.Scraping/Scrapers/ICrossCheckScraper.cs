using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scraping.Scrapers
{
    public interface ICrossCheckScraper
    {
        bool CrossCheck(string url, string username, string password, string accountNumber);
    }
}
