using Caliburn.Micro;

namespace Aps.Customer
{
    public class CustomerRepository
    {
        private readonly IEventAggregator eventAggregator;

        public CustomerRepository(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public Aggregates.Customer GetNewCustomer()
        {
            return Aggregates.Customer.CreateCustomer(eventAggregator);
        }
    }
}