﻿using Aps.BillingCompanies;
using Aps.Core.DomainEvents;
using Aps.Customer;
using Aps.IntegrationEvents;
using Caliburn.Micro;

namespace Aps.Core
{
    public class SchedulingEngine : IHandle<ScrapeSessionFailed>,IHandle<BillingCompanyAddedOpenClosedWindow>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly CustomerRepository customerRepository;
        private readonly BillingCompanyRepository billingCompanyRepository;
        private readonly EventIntegrationService messageSendAndReceiver;

        public SchedulingEngine(IEventAggregator eventAggregator, CustomerRepository customerRepository, BillingCompanyRepository billingCompanyRepository, EventIntegrationService messageSendAndReceiver)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            this.customerRepository = customerRepository;
            this.billingCompanyRepository = billingCompanyRepository;
            this.messageSendAndReceiver = messageSendAndReceiver;
        }

        public void Start()
        {
            //messageSendAndReceiver.Subscribe(typeof(NewCustomerBillingCompanyAccount).FullName);
            messageSendAndReceiver.Subscribe(typeof(BillingCompanyAddedOpenClosedWindow).FullName);

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