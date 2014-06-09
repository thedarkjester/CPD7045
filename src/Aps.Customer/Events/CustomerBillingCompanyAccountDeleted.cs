using Seterlund.CodeGuard;
using System;

namespace Aps.Customers.Events
{
    public class CustomerBillingCompanyAccountDeleted
    {
        public Guid CustomerId { get; set; }
        public Guid BillingCompanyId { get; set; }

        public CustomerBillingCompanyAccountDeleted(Guid customerId, Guid billingCompanyId)
        {
            Guard.That(customerId).IsNotEmpty();
            Guard.That(billingCompanyId).IsNotEmpty();

            this.CustomerId = customerId;
            this.BillingCompanyId = billingCompanyId;
        }
    }
}
