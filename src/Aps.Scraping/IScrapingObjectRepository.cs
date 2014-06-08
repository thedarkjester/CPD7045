using Aps.Integration.EnumTypes;
using System;
using System.Collections.Generic;

namespace Aps.Scraping
{
    public interface IScrapingObjectRepository
    {

        ScrapingObject BuildNewScrapingObject(Guid customerId, Guid billingCompanyId, ScrapeSessionTypes scrapeSessionTypes);
        ScrapingObject GetScrapingObjectByCustomerAndBillingCompanyId(Guid customerId, Guid billingCompanyId);
        ScrapingObject GetScrapingObjectByQueueId(Guid queueId);

        IEnumerable<ScrapingObject> GetAllScrapingObjects();
        IEnumerable<ScrapingObject> GetCompletedScrapeQueue();
        IEnumerable<ScrapingObject> GetAllScrapingObjectsByBillingCompanyId(Guid billingCompanyId);

        void ClearCompletedScrapeList();
        void StoreScrapingObject(ScrapingObject scrapingObject);
        void AddScrapingItemToCompletedQueue(ScrapingObject scrapingObject);
        void RemoveScrapingItemFromRepo(ScrapingObject scrapingObject);
        

    }
}