using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scraping
{
    public class ScrapingLog
    {

        Guid scrapeSessionId;
        Guid customerId;
        Guid billingCompanyId;
        string scrapedData;
        bool hasFailed;
        DateTime dateScraped;
        public Guid ScrapeSessionId { get { return scrapeSessionId; } }
        public Guid CustomerId { get { return customerId; } }
        public Guid BillingCompanyId { get { return billingCompanyId; } }
        public string ScrapedData { get { return scrapedData; } }
        public DateTime DateScraped { get { return dateScraped; } }

        public ScrapingLog(Guid scrapeSessionId, Guid customerId, Guid billingCompanyId, bool hasFailed, string scrapedData)
        {

            this.scrapeSessionId = scrapeSessionId;
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            this.scrapedData = scrapedData;
            this.hasFailed = hasFailed;
            this.dateScraped = DateTime.Now;
        }
    }
}
