using Aps.DomainBase;
using Caliburn.Micro;

namespace Aps.ApsCustomer.Aggregates
{
    public class Customer : Aggregate
    {
        private readonly IEventAggregator aggregator;

        private Customer(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
        }

        public static Customer CreateCustomer(IEventAggregator aggregator)
        {
            return new Customer(aggregator);
        }
    }
}
