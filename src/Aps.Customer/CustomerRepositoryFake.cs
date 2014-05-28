using Aps.ApsCustomer.Aggregates;
using Caliburn.Micro;

namespace Aps.ApsCustomer
{
    public class CustomerRepositoryFake
    {
        private readonly IEventAggregator eventAggregator;

        public CustomerRepositoryFake(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public Customer GetNewCustomer()
        {
            return Customer.CreateCustomer(eventAggregator);
        }

        public Customer GetCustomerById()
        {
            throw new System.NotImplementedException();
        }
    }
}