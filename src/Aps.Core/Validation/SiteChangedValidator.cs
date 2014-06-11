using Aps.Core.Validation;
using Aps.Integration.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.Validation
{
    public class SiteChangedValidator:IValidator
    {

        public void Validate(IList<InterpretedScrapeSessionDataPair> interpretedScrapeSessionDataPair, Guid customerId, Guid billingCompanyId)
        {
            var errors = interpretedScrapeSessionDataPair.Where(a => a.keyValuePair.Key.Equals("Error Code", StringComparison.OrdinalIgnoreCase));

            if (errors.Count() > 0)
            {
                if (interpretedScrapeSessionDataPair.Where(a => a.keyValuePair.Value.Equals(Convert.ToString((int)ScrapingErrorResponseCodes.BrokenScript))).Count() > 0)
                {
                    DataScraperException exception = new DataScraperException(Integration.EnumTypes.ScrapingErrorResponseCodes.BrokenScript);
                    throw exception;
                }
            }
        }
    }
}
