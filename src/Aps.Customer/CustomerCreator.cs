using Aps.Customers.Aggregates;
using Aps.Customers.ValueObjects;
using Caliburn.Micro;
using Seterlund.CodeGuard;

namespace Aps.Customers
{
    public class CustomerCreator
    {
        private readonly IEventAggregator eventAggregator;
        
        public CustomerCreator(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public Customer GetNewCustomer(CustomerFirstName customerFirstName, CustomerLastName customerLastName, CustomerEmailAddress customerEmailAddress, CustomerTelephone customerTelephone, CustomerAPSUsername customerAPSUsername, CustomerAPSPassword customerAPSPassword)
        {
            Guard.That(customerFirstName).IsNotNull();
            Guard.That(customerLastName).IsNotNull();
            Guard.That(customerEmailAddress).IsNotNull();
            Guard.That(customerTelephone).IsNotNull();
            Guard.That(customerAPSUsername).IsNotNull();
            Guard.That(customerAPSPassword).IsNotNull();

            return new Customer(eventAggregator, customerFirstName, customerLastName, customerEmailAddress, customerTelephone, customerAPSUsername, customerAPSPassword);
        }
    }
}
