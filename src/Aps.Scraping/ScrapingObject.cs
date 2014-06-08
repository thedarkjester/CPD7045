using Aps.Integration.EnumTypes;
using System;


namespace Aps.Scraping
{
    public class ScrapingObject
    {
        public Guid queueId;
        public Guid customerId;
        public Guid billingCompanyId;
        public string scrapeStatus;
        public DateTime createdDate;
        public DateTime ScheduledDate;
        public ScrapeSessionTypes scrapeSessionTypes;

        public ScrapingObject(Guid customerId, Guid billingCompanyId, ScrapeSessionTypes scrapeSessionTypes)
        {
            this.queueId = Guid.NewGuid();
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            this.createdDate = DateTime.UtcNow;
            this.ScheduledDate = DateTime.UtcNow;
            this.scrapeSessionTypes = scrapeSessionTypes;

        }
    }
}