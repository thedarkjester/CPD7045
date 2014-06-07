using Aps.Scraping;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Fakes
{
    public class ScrapeLoggingRepositoryFake : IScrapeLoggingRepository
    {

        private readonly List<ScrapingLog> scrapingLogs;

        private readonly IEventAggregator eventAggregator;

        public ScrapeLoggingRepositoryFake(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.scrapingLogs = new List<ScrapingLog>();
        }


        public void StoreScrape(ScrapingLog scrapingLog)
        {
            scrapingLogs.Add(scrapingLog);
        }

    }
}
