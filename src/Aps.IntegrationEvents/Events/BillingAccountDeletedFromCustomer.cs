using Seterlund.CodeGuard;
using System;

namespace Aps.Integration.Events
{
    public class BillingAccountDeletedFromCustomer
    {
        public Guid CustomerId { get; set; }
        public Guid BillingCompanyId { get; set; }

        public BillingAccountDeletedFromCustomer(Guid customerId, Guid billingCompanyId)
        {
            Guard.That(customerId).IsNotEmpty();
            Guard.That(billingCompanyId).IsNotEmpty();

            this.CustomerId = customerId;
            this.BillingCompanyId = billingCompanyId;
        }
    }
}
