using System;
namespace Aps.Scheduling.ApplicationService.InternalEvents
{
    public class ScrapeSessionFailed
    {
        private Guid queueId;
        private Integration.EnumTypes.ScrapingErrorResponseCodes scrapingErrorResponseCode;

        public Guid QueueId { get { return queueId; } }
        public Integration.EnumTypes.ScrapingErrorResponseCodes ScrapingErrorResponseCode { get { return scrapingErrorResponseCode; } }

        public ScrapeSessionFailed(Guid queueId, Integration.EnumTypes.ScrapingErrorResponseCodes scrapingErrorResponseCode)
        {
            this.queueId = queueId;
            this.scrapingErrorResponseCode = scrapingErrorResponseCode;
        }
    }
}