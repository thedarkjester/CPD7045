﻿using Aps.Customers;
using Aps.Customers.Aggregates;
using Aps.Customers.ValueObjects;
using Aps.Integration;
using Aps.Integration.Events;
using Aps.Integration.Queries.CustomerQueries.Dtos;
using Caliburn.Micro;
using System;
using System.Threading;
using Aps.Customers.Events;

namespace Aps.CustomerEventListenerService
{
    public class CustomerService : IHandle<CustomerScrapeSessionFailed>,IHandle<AccountStatementGenerated>
    {
        private readonly EventIntegrationService eventIntegrationService;
        private readonly IEventAggregator eventAggregator;
        private readonly ICustomerRepository customerRepository;

        public CustomerService(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService,ICustomerRepository customerRepository)
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
            
          Customer customer = customerRepository.GetCustomerById(message.customerId);
          customer.ChangeCustomerBillingCompanyAccountStatus(message.billingCompanyId, message.status);
          
        }


        public void Handle(AccountStatementGenerated message)
        {
               
            // is the statement an overall statement (outside BCA's - as is.) or per billing company (need bc id then and in BCA's)?
            // store on customer a statementId and date ( CustomerStatement Value Object )

            Customer customer = customerRepository.GetCustomerById(message.CustomerId);
            customer.SetCustomerStatement(new CustomerStatement(message.AccountStatementId, message.StatementDate));

        }

        public void Start(System.Threading.CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Waiting for EventAggregator Events");
                Thread.Sleep(1000);
            }
        }
    }
}
