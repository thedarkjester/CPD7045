using Aps.Customers;
using Aps.Customers.ValueObjects;
using Aps.Integration;
using Aps.Integration.Events;
using Caliburn.Micro;
using System;
using System.Threading;

namespace Aps.Customer.ApplicationService
{
    public class CustomerService : IHandle<CustomerScrapeSessionFailed>,IHandle<AccountStatementGenerated>, IHandle<Aps.Customers.Events.BillingAccountAddedToCustomer>
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
               
            // is the statement an overall statement (outside BCA's - as is.) or per billing company (need bc id then and in BCA's)?
            // store on customer a statementId and date ( CustomerStatement Value Object )

            Customers.Aggregates.Customer customer = customerRepository.GetCustomerById(message.CustomerId);
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

        public void Handle(Customers.Events.BillingAccountAddedToCustomer message)
        {
            eventIntegrationService.Publish(new Aps.Integration.Events.CustomerBillingAccountAdded(message.CustomerId, message.BillingCompanyId));
        }
    }
}
