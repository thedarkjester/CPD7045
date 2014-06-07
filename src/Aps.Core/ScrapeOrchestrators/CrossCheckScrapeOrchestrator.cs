using Aps.Core.InternalEvents;
using Aps.Integration;
using Aps.Integration.Events;
using Aps.Scraping.Scrapers;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core.ScrapeOrchestrators
{
    public class CrossCheckScrapeOrchestrator : ScrapeOrchestrator
    {
        readonly IEventAggregator eventAggregator;
        readonly FailureHandler failureHandler;
        readonly EventIntegrationService eventIntegrationService;
        readonly ICrossCheckScraper crossCheckScraper;
        public CrossCheckScrapeOrchestrator(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService, FailureHandler failureHandler, ICrossCheckScraper crossCheckScraper)
        {
            this.eventAggregator = eventAggregator;
            this.eventIntegrationService = eventIntegrationService;
            this.failureHandler = failureHandler;
            this.crossCheckScraper = crossCheckScraper;
        }

        public override void Orchestrate()
        {
            Guid crossCheckSessionId = Guid.NewGuid();
            
            eventIntegrationService.Publish(new CrossCheckSessionStarted(crossCheckSessionId, Guid.NewGuid(), Guid.NewGuid(), null));
            bool crossCheckSuccessful = crossCheckScraper.CrossCheck(null, null, null, null);
            if (crossCheckSuccessful)
            {
                eventIntegrationService.Publish(new CrossCheckSessionCompletedSuccesfully(crossCheckSessionId, Guid.NewGuid(), Guid.NewGuid(), null));
                eventAggregator.Publish(new CrossCheckCompleted(Guid.NewGuid()));
                return;
            }
            eventIntegrationService.Publish(new CrossCheckSessionCompletedWithErrors(crossCheckSessionId, Guid.NewGuid(), Guid.NewGuid(), null, "Account Number invalid"));
            eventAggregator.Publish(new CrossCheckCompleted(Guid.NewGuid()));
        }
    }
}
