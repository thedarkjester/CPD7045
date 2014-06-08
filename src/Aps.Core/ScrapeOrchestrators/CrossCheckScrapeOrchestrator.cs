using Aps.Integration;
using Aps.Integration.Events;
using Aps.Scheduling.ApplicationService.Entities;
using Aps.Scheduling.ApplicationService.InternalEvents;
using Aps.Scraping.Scrapers;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.ScrapeOrchestrators
{
    public class CrossCheckScrapeOrchestrator : ScrapeOrchestrator
    {
        readonly IEventAggregator eventAggregator;
        readonly EventIntegrationService eventIntegrationService;
        readonly ICrossCheckScraper crossCheckScraper;
        public CrossCheckScrapeOrchestrator(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService, ICrossCheckScraper crossCheckScraper)
        {
            this.eventAggregator = eventAggregator;
            this.eventIntegrationService = eventIntegrationService;
            this.crossCheckScraper = crossCheckScraper;
        }

        public override void Orchestrate(ScrapeOrchestratorEntity scrapeOrchestratorEntity)
        {
            Guid crossCheckSessionId = Guid.NewGuid();
            
            eventIntegrationService.Publish(new CrossCheckSessionStarted(crossCheckSessionId, Guid.NewGuid(), Guid.NewGuid(), null));
            bool crossCheckSuccessful = crossCheckScraper.CrossCheck(scrapeOrchestratorEntity.Url, scrapeOrchestratorEntity.Username, scrapeOrchestratorEntity.Password, scrapeOrchestratorEntity.AccountNumber);
            if (crossCheckSuccessful)
            {
                eventIntegrationService.Publish(new CrossCheckSessionCompletedSuccesfully(crossCheckSessionId, scrapeOrchestratorEntity.CustomerId, scrapeOrchestratorEntity.BillingCompanyId, scrapeOrchestratorEntity.AccountNumber));
                eventAggregator.Publish(new CrossCheckCompleted(scrapeOrchestratorEntity.QueueId, true));
                return;
            }
            eventIntegrationService.Publish(new CrossCheckSessionCompletedWithErrors(crossCheckSessionId, scrapeOrchestratorEntity.CustomerId, scrapeOrchestratorEntity.BillingCompanyId, null, "Account Number invalid"));
            eventAggregator.Publish(new CrossCheckCompleted(scrapeOrchestratorEntity.QueueId, false));
        }
    }
}
