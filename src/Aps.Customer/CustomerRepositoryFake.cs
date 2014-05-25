using Caliburn.Micro;

namespace Aps.Customer
{
    public class CustomerRepositoryFake
    {
        private readonly IEventAggregator eventAggregator;

        public CustomerRepositoryFake(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public Aggregates.Customer GetNewCustomer()
        {
            return Aggregates.Customer.CreateCustomer(eventAggregator);
        }
    }
}