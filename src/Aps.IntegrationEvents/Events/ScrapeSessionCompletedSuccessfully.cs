using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Integration.Events
{
    [Serializable]
    public class ScrapeSessionCompletedSuccessfully
    {
        Guid scrapeSessionId;
        Guid customerId;
        Guid billingCompanyId;
        DateTime dateCreated;

        public Guid ScrapeSessionId { get { return scrapeSessionId; } }
        public Guid CustomerId { get { return customerId; } }
        public Guid BillingCompanyId { get { return billingCompanyId; } }
        public DateTime DateCreated { get { return dateCreated; } }
        public ScrapeSessionCompletedSuccessfully(Guid scrapeSessionId, Guid customerId, Guid billingCompanyId)
        {

            this.scrapeSessionId = scrapeSessionId;
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            dateCreated = DateTime.Now;
        }
    }
}
