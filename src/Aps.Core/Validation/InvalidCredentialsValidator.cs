﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core.Validation
{
    public class InvalidCredentialsValidator : IValidator
    {
        public void Validate(IList<InterpretedScrapeSessionDataPair> interpretedScrapeSessionDataPair, Guid customerId, Guid billingCompanyId)
        {
            var errors = interpretedScrapeSessionDataPair.Where(a => a.keyValuePair.Key.Equals("Error Code", StringComparison.OrdinalIgnoreCase));

            if (errors.Count() > 0)
            {
                if (interpretedScrapeSessionDataPair.Where(a => a.keyValuePair.Value.Equals("2")).Count() > 0)
                {
                    DataScraperException exception = new DataScraperException(Integration.EnumTypes.ScrapingErrorResponseCodes.InvalidCredentials);
                    throw exception;
                }
            }
        }
    }
}