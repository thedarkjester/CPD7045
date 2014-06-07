using System;
using System.Collections.Generic;

namespace Aps.Scraping
{
    public interface IScrapingObjectRepository
    {
        void StoreScrapingObject(ScrapingObject scrapingObject);
        ScrapingObject BuildNewScrapingObject(Guid customerId, Guid billingCompanyId, string URL, string plainText, string hiddenText);
        ScrapingObject GetScrapingObjectByCustomerAndBillingCompanyId(Guid customerId, Guid billingCompanyId);
        IEnumerable<ScrapingObject> GetAllScrapingObjectsByBillingCompanyId();
    }
}