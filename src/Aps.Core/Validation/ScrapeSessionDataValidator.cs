using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aps.Scheduling.ApplicationService.Validation
{
    public class ScrapeSessionDataValidator
    {
        readonly IEnumerable<IValidator> validators;

        public ScrapeSessionDataValidator(IEnumerable<IValidator> validatorList)
        {
            validators = validatorList;
        }

        public void ValidateScrapeData(IList<InterpretedScrapeSessionDataPair> interpretedScrapeSessionDataPair, Guid customerId, Guid billingCompanyId)
        {
            foreach (var validator in validators)
            {
                validator.Validate(interpretedScrapeSessionDataPair,customerId,billingCompanyId);
            }
        }
    }
}
