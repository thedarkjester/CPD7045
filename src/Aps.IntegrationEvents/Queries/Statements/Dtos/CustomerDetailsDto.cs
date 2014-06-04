using System;
using Seterlund.CodeGuard;

namespace Aps.Integration.Queries.Statements.Dtos
{
    public class CustomerDetailsDto
    {
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }

        public CustomerDetailsDto(Guid customerId, string customerName)
        {
            Guard.That(customerName).IsNotNull();
            Guard.That(customerName).IsNotEmpty();
            Guard.That(customerId).IsNotEmpty();

            CustomerId = customerId;
            CustomerName = customerName;
        }
    }
}