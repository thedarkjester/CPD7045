using System;

namespace Aps.Scraping
{
    public class ScrapingObject
    {
        public Guid queueId;
        public Guid customerId;
        public Guid billingCompanyId;
        public string scrapeType;
        public string scrapeStatus;
        public DateTime createdDate;
        public DateTime ScheduledDate;
        public bool registrationType;

        public ScrapingObject(Guid customerId, Guid billingCompanyId, bool registrationType)
        {
            this.queueId = Guid.NewGuid();
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            this.registrationType = registrationType;
            this.createdDate = DateTime.UtcNow;
            this.ScheduledDate = DateTime.UtcNow;
        }
    }
}