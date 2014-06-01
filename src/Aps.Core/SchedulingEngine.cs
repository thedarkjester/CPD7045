using Aps.BillingCompanies;
using Aps.Core.InternalEvents;
using Aps.Customers;
using Aps.IntegrationEvents;
using Aps.ApsCustomer;
using Aps.Integration;
using Caliburn.Micro;

namespace Aps.Core
{
    public class SchedulingEngine : IHandle<ScrapeSessionFailed>,IHandle<BillingCompanyAddedOpenClosedWindow>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly CustomerRepositoryFake customerRepositoryFake;
        private readonly BillingCompanyRepositoryFake billingCompanyRepositoryFake;
        private readonly EventIntegrationService messageSendAndReceiver;

        public SchedulingEngine(IEventAggregator eventAggregator, CustomerRepositoryFake customerRepositoryFake, BillingCompanyRepositoryFake billingCompanyRepositoryFake, EventIntegrationService messageSendAndReceiver)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            this.customerRepositoryFake = customerRepositoryFake;
            this.billingCompanyRepositoryFake = billingCompanyRepositoryFake;
            this.messageSendAndReceiver = messageSendAndReceiver;
        }

        public void Start()
        {
            //messageSendAndReceiver.SubscribeToEventByNameSpace(typeof(NewCustomerBillingCompanyAccount).FullName);
            messageSendAndReceiver.SubscribeToEventByNameSpace(typeof(BillingCompanyAddedOpenClosedWindow).FullName);

            var session = new ScrapeSession();

            // every so often look for scrape sessions that are valid ( retry or otherwise )
            Scrape();
        }

        private void Scrape()
        {
              // DO SOMETHING AND IF FAILS
                eventAggregator.Publish(new ScrapeSessionFailed());
        }

        public void Stop()
        {

        }

        public void Handle(ScrapeSessionFailed message)
        {
            // Failurehandler code
            messageSendAndReceiver.Publish(new ScrapeSessionFailedEvent());
        }

        public void Handle(BillingCompanyAddedOpenClosedWindow message)
        {
            // modify sessions
        }
    }

    public class ScrapeSession
    {
    }
}