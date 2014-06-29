using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Caliburn.Micro;
using Seterlund.CodeGuard;

namespace Aps.BillingCompanies
{
    public class BillingCompanyFactory
    {
        private readonly IEventAggregator eventAggregator;

        public BillingCompanyFactory(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public BillingCompany ConstructBillingCompanyWithGivenValues(BillingCompanyName billingCompanyName, BillingCompanyType billingCompanyType, BillingCompanyScrapingUrl billingCompanyScrapingUrl, BillingCompanyCrossCheckScrapeEnabled crossCheckScrapeEnabled = null)
        {
            Guard.That(billingCompanyName).IsNotNull();
            Guard.That(billingCompanyType).IsNotNull();
            Guard.That(billingCompanyScrapingUrl).IsNotNull();

            var validCrossCheckScrapeEnabled = crossCheckScrapeEnabled;
            if (validCrossCheckScrapeEnabled == null)
            {
                validCrossCheckScrapeEnabled = new BillingCompanyCrossCheckScrapeEnabled(false);
            }

            return new BillingCompany(eventAggregator, billingCompanyName, billingCompanyType, billingCompanyScrapingUrl, validCrossCheckScrapeEnabled);
        }
    }
}