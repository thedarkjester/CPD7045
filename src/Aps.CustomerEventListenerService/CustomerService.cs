﻿using Aps.Customers;
using Aps.Customers.Aggregates;
using Aps.Integration;
using Aps.Integration.Events;
using Aps.Integration.Events;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aps.CustomerEventListenerService
{
    public class CustomerService : IHandle<CustomerScrapeSessionFailed>,IHandle<AccountStatementGenerated>
    {
        private readonly EventIntegrationService eventIntegrationService;
        private readonly IEventAggregator eventAggregator;
        private readonly CustomerRepositoryFake customerRepository;

        public CustomerService(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService,CustomerRepositoryFake customerRepository)
        {
            this.eventIntegrationService = eventIntegrationService;
            this.eventAggregator = eventAggregator;
            this.customerRepository = customerRepository;

            eventAggregator.Subscribe(this);

            
            this.eventIntegrationService.SubscribeToEventByNameSpace(typeof(CustomerScrapeSessionFailed).FullName);
            this.eventIntegrationService.SubscribeToEventByNameSpace(typeof(AccountStatementGenerated).FullName);
        }

        public void Handle(CustomerScrapeSessionFailed message)
        {
           // Customer customer = customerRepository.GetCustomerById();
          //  customer.SetStatus();
        }


        public void Handle(AccountStatementGenerated message)
        {
            // store on customer a statementId and date ( CustomerStatement Value Object )
        }

        public void Start(System.Threading.CancellationToken cancellationToken)
        {
            while (true)
            {
                Console.WriteLine("Waiting for EventAggregator Events");
                Thread.Sleep(1000);
            }
        }
    }
}
