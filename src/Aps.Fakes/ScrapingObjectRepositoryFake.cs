using System;
using System.Collections.Generic;
using System.Linq;
using Aps.AccountStatements;
using Aps.Scraping;
using Caliburn.Micro;

namespace Aps.Fakes
{
    public class ScrapingObjectRepositoryFake : IScrapingObjectRepository
    {
        private readonly List<ScrapingObject> scrapingObjects;

        private readonly IEventAggregator eventAggregator;
        private readonly ScrapingObjectCreator scrapingObjectCreator;

        public ScrapingObjectRepositoryFake(IEventAggregator eventAggregator, ScrapingObjectCreator scrapingObjectCreator)
        {
            this.eventAggregator = eventAggregator;
            this.scrapingObjectCreator = scrapingObjectCreator;
            this.scrapingObjects = new List<ScrapingObject>();
        }

        public void StoreScrapingObject(ScrapingObject scrapingObject)
        {
            this.scrapingObjects.Add(scrapingObject);
        }

        public ScrapingObject BuildNewScrapingObject(Guid customerId, Guid billingCompanyId, string URL, string plainText, string hiddenText)
        {
            return this.scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, URL, plainText, hiddenText);
        }

        public ScrapingObject GetScrapingObjectByCustomerAndBillingCompanyId(Guid customerId, Guid billingCompanyId)
        {
            return this.scrapingObjects.FirstOrDefault(x => ((x.customerId == customerId) && (x.billingCompanyId == billingCompanyId)));
        }

        public IEnumerable<ScrapingObject> GetAllScrapingObjectsByBillingCompanyId()
        {
            return scrapingObjects; // currently returns all objects icorreclty - Need to fix it
        }

    }
}
