using Aps.Customers;
using Aps.Customers.ValueObjects;
using Aps.Customers.Events;
using Aps.Integration;
using Aps.Integration.Events;
using Caliburn.Micro;
using System;
using System.Threading;

namespace Aps.Customer.ApplicationService
{
    public class CustomerService : IHandle<CustomerScrapeSessionFailed>, IHandle<AccountStatementGenerated>, IHandle<BillingAccountAddedToCustomer>,
                                   IHandle<CrossCheckSessionCompletedWithErrors>, IHandle<CustomerBillingCompanyAccountDeleted>
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
            
          Customers.Aggregates.Customer customer = customerRepository.GetCustomerById(message.customerId);
          customer.ChangeCustomerBillingCompanyAccountStatus(message.billingCompanyId, message.status);
          
        }


        public void Handle(AccountStatementGenerated message)
        {
            
            Customers.Aggregates.Customer customer = customerRepository.GetCustomerById(message.CustomerId);
            customer.SetCustomerStatement(new CustomerStatement(message.AccountStatementId, message.StatementDate));

        }

        public void Start(System.Threading.CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Waiting for EventAggregator Events");
                Console.WriteLine("Pausing, and then redistributing internal events");
                Thread.Sleep(1000);
            }
        }

        public void Handle(BillingAccountAddedToCustomer message)
        {
            eventIntegrationService.Publish(new CustomerBillingAccountAdded(message.CustomerId, message.BillingCompanyId));
        }

        public void Handle(CrossCheckSessionCompletedWithErrors message)
        {
            Customers.Aggregates.Customer customer = customerRepository.GetCustomerById(message.CustomerId);
            customer.ChangeCustomerBillingCompanyAccountStatus(message.BillingCompanyId, "Inactive");
        }

        public void Handle(CustomerBillingCompanyAccountDeleted message)
        {
            eventIntegrationService.Publish(new BillingAccountDeletedFromCustomer(message.CustomerId, message.BillingCompanyId));
        }
    }
}
