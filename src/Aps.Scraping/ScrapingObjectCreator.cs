using System;
using Caliburn.Micro;

namespace Aps.Scraping
{
    public class ScrapingObjectCreator
    {
        private readonly IEventAggregator eventAggregator;

        public ScrapingObjectCreator(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public ScrapingObject GetNewScrapingObject(Guid customerId, Guid billingCompanyId, bool registrationType)
        {
            return new ScrapingObject(customerId, billingCompanyId, registrationType);
        }
    }
}
