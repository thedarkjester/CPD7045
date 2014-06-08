using Caliburn.Micro;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Customers.Events
{
    public class CustomerBillingAccountAdded
    {
        public Guid customerId { get; set; }
        public Guid billingCompanyId { get; set; }

        public CustomerBillingAccountAdded(Guid customerId, Guid billingCompanyId)
        {
            Guard.That(customerId).IsNotEmpty();
            Guard.That(billingCompanyId).IsNotEmpty();

            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
        }
    }
}
