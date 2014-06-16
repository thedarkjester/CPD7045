using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.InternalEvents
{
    public class ScrapingScriptUpdated
    {
        Guid billingId;
        public Guid BillingId { get { return billingId; } }

        public ScrapingScriptUpdated(Guid billingId)
        {
            this.billingId = billingId;
        }
    }
}
