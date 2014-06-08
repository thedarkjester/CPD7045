using System;
namespace Aps.Scheduling.ApplicationService.InternalEvents
{
    public class ScrapeSessionFailed
    {
        private Guid customerId;
        private Guid billingCompanyId;
        private Integration.EnumTypes.ScrapingErrorResponseCodes scrapingErrorResponseCodes;

        public ScrapeSessionFailed(Guid queueId, string failureReason)
        {

        }

        public ScrapeSessionFailed(Guid customerId, Guid billingCompanyId, Integration.EnumTypes.ScrapingErrorResponseCodes scrapingErrorResponseCodes)
        {
            // TODO: Complete member initialization
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            this.scrapingErrorResponseCodes = scrapingErrorResponseCodes;
        }
    }
}