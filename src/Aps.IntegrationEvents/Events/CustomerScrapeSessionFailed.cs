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
    public class CustomerScrapeSessionFailed
    {

        public Guid customerId { get; set; }
        public Guid billingCompanyId { get; set; }
        public ScrapingErrorResponseCodes errorNum { get; set; }
        public string status { get; set; }


        public CustomerScrapeSessionFailed(Guid customerId, Guid billingCompanyId, ScrapingErrorResponseCodes errorNum, string status)
        {
            Guard.That(customerId).IsNotEmpty();
            Guard.That(billingCompanyId).IsNotEmpty();
            Guard.That(status).IsNotNullOrEmpty();

            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            this.errorNum = errorNum;
            this.status = status;

        }

    }
}
