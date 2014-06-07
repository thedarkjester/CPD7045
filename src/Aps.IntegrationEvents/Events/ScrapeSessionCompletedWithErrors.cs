using Aps.Integration.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Integration.Events
{
    public class ScrapeSessionCompletedWithErrors
    {
        Guid scrapeSessionId;
        Guid customerId;
        Guid billingCompanyId;
        DateTime dateCreated;
        string error;

        public Guid ScrapeSessionId { get { return scrapeSessionId; } }
        public Guid CustomerId { get { return customerId; } }
        public Guid BillingCompanyId { get { return billingCompanyId; } }
        public DateTime DateCreated { get { return dateCreated; } }
        public string Error { get { return error; } }
        public ScrapeSessionCompletedWithErrors(Guid scrapeSessionId, Guid customerId, Guid billingCompanyId, string error)
        {
            this.scrapeSessionId = scrapeSessionId;
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            this.error = error;
            dateCreated = DateTime.Now;
        }
    }
}
