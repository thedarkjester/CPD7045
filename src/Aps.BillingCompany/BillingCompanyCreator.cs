using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Caliburn.Micro;
using Seterlund.CodeGuard;

namespace Aps.BillingCompanies
{
    public class BillingCompanyCreator
    {
        private readonly IEventAggregator eventAggregator;

        public BillingCompanyCreator(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public BillingCompany GetNewBillingCompany(BillingCompanyName billingCompanyName, BillingCompanyType billingCompanyType, BillingCompanyScrapingUrl billingCompanyScrapingUrl,bool crossCheckScrapeEnabled)
        {
            Guard.That(billingCompanyName).IsNotNull();
            Guard.That(billingCompanyType).IsNotNull();
            Guard.That(billingCompanyScrapingUrl).IsNotNull();

            return new BillingCompany(eventAggregator, billingCompanyName, billingCompanyType, billingCompanyScrapingUrl, crossCheckScrapeEnabled);
        }
    }
}