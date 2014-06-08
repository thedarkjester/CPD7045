using Aps.Integration.EnumTypes;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Queries.CustomerQueries;
using Aps.Integration.Queries.CustomerQueries.Dtos;
using Caliburn.Micro;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Integration.Events
{
    [Serializable]
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
