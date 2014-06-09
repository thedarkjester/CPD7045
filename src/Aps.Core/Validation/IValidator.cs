using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core.Validation
{
    public interface IValidator
    {
        void Validate(IList<InterpretedScrapeSessionDataPair> interpretedScrapeSessionDataPair, Guid customerId, Guid billingCompanyId);
    }
}
