using Aps.BillingCompanies.Aggregates;
using Caliburn.Micro;

namespace Aps.BillingCompanies
{
    public class BillingCompanyRepositoryFake
    {
        private readonly IEventAggregator eventAggregator;

        public BillingCompanyRepositoryFake(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        private int lastProcessedEvent = 0;

        public BillingCompany GetNewBillingCompany()
        {
            return new BillingCompany(eventAggregator);
        }
    }
}