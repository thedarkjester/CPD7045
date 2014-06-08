using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aps.Core.Validation
{
    public class ScrapeSessionDataValidator
    {

        public void validateScrapeData(IList<InterpretedScrapeSessionDataPair> interpretedScrapeSessionDataPair)
        {
            var errors = interpretedScrapeSessionDataPair.Where(a => a.keyValuePair.Key.Equals("Error Code", StringComparison.OrdinalIgnoreCase));

            if (errors.Count() > 0)
            {
                if (interpretedScrapeSessionDataPair.Where(a => a.keyValuePair.Value.Equals("2")).Count() > 0)
                {
                    ScrapeValidationException exception = new ScrapeValidationException();
                    exception.ErrorCode = ErrorCode.InvalidCredentials;
                    throw exception;
                }
            }
        }
    }
}
