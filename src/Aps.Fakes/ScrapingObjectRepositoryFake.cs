﻿using System;
using System.Collections.Generic;
using System.Linq;
using Aps.AccountStatements;
using Aps.Scraping;
using Caliburn.Micro;
using Aps.Integration.EnumTypes;

namespace Aps.Fakes
{
    public class ScrapingObjectRepositoryFake : IScrapingObjectRepository
    {
        public readonly List<ScrapingObject> scrapingMasterQueue;
        public readonly List<ScrapingObject> scrapingCompletedQueue;

        public readonly IEventAggregator eventAggregator;
        public readonly ScrapingObjectCreator scrapingObjectCreator;

        public ScrapingObjectRepositoryFake(IEventAggregator eventAggregator, ScrapingObjectCreator scrapingObjectCreator)
        {
            this.eventAggregator = eventAggregator;
            this.scrapingObjectCreator = scrapingObjectCreator;
            this.scrapingMasterQueue = new List<ScrapingObject>();
            this.scrapingCompletedQueue = new List<ScrapingObject>();
        }

        public void StoreScrapingObject(ScrapingObject scrapingObject)
        {
            this.scrapingMasterQueue.Add(scrapingObject);
        }

        public void AddScrapingItemToCompletedQueue(ScrapingObject scrapingObject)
        {
            this.scrapingCompletedQueue.Add(scrapingObject);
        }

        public void RemoveScrapingItemFromRepo(ScrapingObject scrapingObject)
        {
            this.scrapingMasterQueue.Remove(scrapingObject);
        }

        public ScrapingObject BuildNewScrapingObject(Guid customerId, Guid billingCompanyId, ScrapeSessionTypes scrapeSessionTypes)
        {
            return this.scrapingObjectCreator.GetNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);
        }

        public ScrapingObject GetScrapingObjectByCustomerAndBillingCompanyId(Guid customerId, Guid billingCompanyId)
        {
            return this.scrapingMasterQueue.FirstOrDefault(x => ((x.customerId == customerId) && (x.billingCompanyId == billingCompanyId)));
        }

        public ScrapingObject GetScrapingObjectByQueueId(Guid queueId)
        {
            return this.scrapingMasterQueue.FirstOrDefault(x => (x.queueId == queueId));
        }

        public IEnumerable<ScrapingObject> GetAllScrapingObjects()
        {
            //scrapingObjects.

            return scrapingMasterQueue;
        }

        public IEnumerable<ScrapingObject> GetCompletedScrapeQueue()
        {
            //scrapingObjects.

            return scrapingCompletedQueue;
        }

        public void ClearCompletedScrapeList()
        {
            scrapingCompletedQueue.Clear();
        }

       // IEnumerable<ScrapingObject> GetAllScrapingObjectsByBillingCompanyId(Guid billingCompanyId);

        public IEnumerable<ScrapingObject> GetAllScrapingObjectsByBillingCompanyId(Guid billingCompanyId)
        {
            return scrapingMasterQueue; // currently returns all objects incorreclty - Need to fix it
        }

    }
}
