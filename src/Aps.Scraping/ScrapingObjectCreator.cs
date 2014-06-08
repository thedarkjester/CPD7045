using System;
using Caliburn.Micro;
using Aps.Integration.EnumTypes;

namespace Aps.Scraping
{
    public class ScrapingObjectCreator
    {
        private readonly IEventAggregator eventAggregator;

        public ScrapingObjectCreator(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public ScrapingObject GetNewScrapingObject(Guid customerId, Guid billingCompanyId, ScrapeSessionTypes scrapeSessionTypes)
        {
            return new ScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
        }
    }
}
