using Aps.BillingCompanies;
using Aps.Core.InternalEvents;
using Aps.Customers;
using Aps.Integration;
using Aps.Integration.Events;
using Caliburn.Micro;

namespace Aps.Core
{
    public class SchedulingEngine : IHandle<ScrapeSessionFailed>,IHandle<BillingCompanyAddedOpenClosedWindow>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ICustomerRepository ICustomerRepository;
        private readonly IBillingCompanyRepository billingCompanyRepository;
        private readonly EventIntegrationService messageSendAndReceiver;

        public SchedulingEngine(IEventAggregator eventAggregator, ICustomerRepository ICustomerRepository, IBillingCompanyRepository billingCompanyRepository, EventIntegrationService messageSendAndReceiver)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            this.ICustomerRepository = ICustomerRepository;
            this.billingCompanyRepository = billingCompanyRepository;
            this.messageSendAndReceiver = messageSendAndReceiver;
        }

        public void Start()
        {
            //messageSendAndReceiver.SubscribeToEventByNameSpace(typeof(NewCustomerBillingCompanyAccount).FullName);
            messageSendAndReceiver.SubscribeToEventByNameSpace(typeof(BillingCompanyAddedOpenClosedWindow).FullName);


            // every so often look for scrape sessions that are valid ( retry or otherwise )
            Scrape();
        }

        private void Scrape()
        {
              // DO SOMETHING AND IF FAILS
                eventAggregator.Publish(new ScrapeSessionFailed(System.Guid.NewGuid(), string.Empty));
        }

        public void Stop()
        {

        }

        public void Handle(ScrapeSessionFailed message)
        {
            // Failurehandler code
            messageSendAndReceiver.Publish(new ScrapeSessionFailedEvent(System.Guid.NewGuid(), string.Empty));
        }

        public void Handle(BillingCompanyAddedOpenClosedWindow message)
        {
            // modify sessions
        }
    }
}