using Aps.Integration.EnumTypes;
using System;


namespace Aps.Scraping
{
    public class ScrapingObject
    {
        /*
        public Guid queueId;
        public Guid customerId;
        public Guid billingCompanyId;
        public string scrapeStatus;
        public DateTime createdDate;
        public DateTime ScheduledDate;
        public ScrapeSessionTypes scrapeSessionTypes;
         * */

        private Guid queueId;
        private Guid customerId;
        private Guid billingCompanyId;
        private string scrapeStatus;
        private DateTime createdDate;
        private DateTime scheduledDate;
        private ScrapeSessionTypes scrapeSessionTypes;

        public ScrapingObject(Guid customerId, Guid billingCompanyId, ScrapeSessionTypes scrapeSessionTypes)
        {
            this.queueId = Guid.NewGuid();
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            this.createdDate = DateTime.UtcNow;
            this.scheduledDate = DateTime.UtcNow;
            this.scrapeSessionTypes = scrapeSessionTypes;
            this.scrapeStatus = "active";
        }

        public Guid QueueId
        {
            get { return queueId; }
        }

        public Guid CustomerId
        {
            get { return customerId; }
        }

        public Guid BillingCompanyId
        {
            get { return billingCompanyId; }
        }

        public ScrapeSessionTypes ScrapeSessionTypes
        {
            get { return scrapeSessionTypes; }
            set { scrapeSessionTypes = value; }
        }

        public string ScrapeStatus
        {
            get { return scrapeStatus; }
            set { scrapeStatus = value; }
        }

        public DateTime ScheduledDate
        {
            get { return scheduledDate; }
            set { scheduledDate = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
        }
    }
}