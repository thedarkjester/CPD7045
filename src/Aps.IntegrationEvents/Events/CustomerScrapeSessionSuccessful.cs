using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Integration.Events
{
    [Serializable]
    public class CustomerScrapeSessionSuccessful
    {

        public CustomerScrapeSessionSuccessful(Guid customerId, Guid billingCompanyId)
        {
            
        }
    }
}
