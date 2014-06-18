using Aps.Integration.Events;
using Aps.Scheduling.ApplicationService.InternalEvents;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService
{
    public class InternalSchedularEventsMock
    {
        private IEventAggregator eventAggregator;
        public InternalSchedularEventsMock(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }

        public void getScrapeSessionFailedEvent(Guid queueId, Integration.EnumTypes.ScrapingErrorResponseCodes scrapingErrorResponseCode)
        {
            ScrapeSessionFailed myEvent = new ScrapeSessionFailed(queueId, scrapingErrorResponseCode);
            eventAggregator.Publish(myEvent);
        }

        public void getCrossCheckCompletedEvent(Guid queueId, bool successful)
        {
            CrossCheckCompleted myEvent = new CrossCheckCompleted(queueId, successful);
            eventAggregator.Publish(myEvent);
        }

        public void getScrapeSessionSuccessfullEvent(Guid queueId, DateTime statementDate)
        {
            ScrapeSessionSuccessfull myEvent = new ScrapeSessionSuccessfull(queueId, statementDate);
            eventAggregator.Publish(myEvent);
        }

        public void getScrapeSessionDuplicateStatementEvent(Guid queueId)
        {
            ScrapeSessionDuplicateStatement myEvent = new ScrapeSessionDuplicateStatement(queueId);
            eventAggregator.Publish(myEvent);
        }

        public void getMaxServerThreadModificationEvent(int maxThreadCount)
        {
            MaxConcurrentServerScrapes myEvent = new MaxConcurrentServerScrapes(maxThreadCount);
            eventAggregator.Publish(myEvent);
        }

        public void getScrapingQueueStrategy(IScrapeQueueingStrategy myEvent)
        {
            eventAggregator.Publish(myEvent);
        }

    }
}
