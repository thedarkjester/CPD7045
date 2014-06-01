using Aps.Integration.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.IntegrationEvents.Events
{
    [Serializable]
    public class CustomerScrapeSessionFailed
    {
        public CustomerScrapeSessionFailed(Guid customerId, Guid billingCompanyId, ScrapingErrorResponseCodes errorNum)
        {
            
        }

    }
}
