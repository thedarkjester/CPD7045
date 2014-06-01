using System;
using System.Linq;
using Aps.Customers.Aggregates;
using Aps.Customers.ValueObjects;
using Caliburn.Micro;
using System.Collections.Generic;
using Seterlund.CodeGuard;

namespace Aps.Customers
{
    public class CustomerRepositoryFake
    {
        private readonly List<Customer> customers;

        private readonly IEventAggregator eventAggregator;
        private readonly CustomerCreator customerCreator;
        
        public CustomerRepositoryFake(IEventAggregator eventAggregator, CustomerCreator customerCreator)
        {
            this.eventAggregator = eventAggregator;
            this.customerCreator = customerCreator;
            this.customers = new List<Customer>();
        }

        public void StoreCustomer(Customer customer)
        {
            // validate Ids?
            this.customers.Add(customer);
        }

        public Customer GetNewCustomer(CustomerFirstName customerFirstName, CustomerLastName customerLastName, CustomerEmailAddress customerEmailAddress, 
                                                  CustomerTelephone customerTelephone, CustomerAPSUsername customerAPSUsername, CustomerAPSPassword customerAPSPassword)
        {
            Guard.That(customerFirstName).IsNotNull();
            Guard.That(customerLastName).IsNotNull();
            Guard.That(customerEmailAddress).IsNotNull();
            Guard.That(customerTelephone).IsNotNull();
            Guard.That(customerAPSUsername).IsNotNull();
            Guard.That(customerAPSPassword).IsNotNull();

            return this.customerCreator.GetNewCustomer(customerFirstName, customerLastName, customerEmailAddress, customerTelephone, customerAPSUsername, customerAPSPassword);
        }

        public Customer GetCustomerById(Guid id)
        {
            return this.customers.FirstOrDefault(x => x.Id == id);
        }
    }
}

             

 