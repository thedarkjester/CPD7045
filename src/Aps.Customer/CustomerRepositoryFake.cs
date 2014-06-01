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

        public Aggregates.Customer GetNewCustomer()
        {
            return Aggregates.Customer.CreateCustomer(eventAggregator);
        }
    }
}

             

 