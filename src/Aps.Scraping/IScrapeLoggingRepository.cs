using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scraping
{
    public interface IScrapeLoggingRepository
    {
        void StoreScrape(ScrapingLog scrapingLog);
    }
}
