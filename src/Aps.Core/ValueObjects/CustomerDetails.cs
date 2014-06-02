using System;
using Seterlund.CodeGuard;

namespace Aps.Core.ValueObjects
{
    public class CustomerDetails
    {
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }

        public CustomerDetails(Guid customerId, string customerName)
        {
            Guard.That(customerName).IsNotNull();
            Guard.That(customerName).IsNotEmpty();
            Guard.That(customerId).IsNotEmpty();

            CustomerId = customerId;
            CustomerName = customerName;
        }
    }
}