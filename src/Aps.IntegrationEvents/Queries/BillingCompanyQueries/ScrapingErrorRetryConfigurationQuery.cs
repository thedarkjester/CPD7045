using System;
using System.Collections.Generic;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Aps.Integration.EnumTypes;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;

namespace Aps.Integration.Queries.BillingCompanyQueries
{
    public class ScrapingErrorRetryConfigurationQuery
    {
        private readonly IBillingCompanyRepository billingCompanyRepository;

        public ScrapingErrorRetryConfigurationQuery(IBillingCompanyRepository billingCompanyRepository)
        {
            this.billingCompanyRepository = billingCompanyRepository;
        }

        private ScrapingErrorRetryConfigurationDto MapcrapingErrorRetryConfigurationTocrapingErrorRetryConfigurationDto(ScrapingErrorRetryConfiguration retryConfiguration)
        {
            var retryConfigurationDto = new ScrapingErrorRetryConfigurationDto
                {
                    ResponseCode =  (ScrapingErrorResponseCodes)retryConfiguration.ResponseCode,
                    RetryInterval = retryConfiguration.RetryInterval
                };

            return retryConfigurationDto;
        }

        public IEnumerable<ScrapingErrorRetryConfigurationDto> GetAllScrapingErrorRetryConfigurations(Guid billingCompanyId)
        {
            List<ScrapingErrorRetryConfigurationDto> billingCompanyDtos = new List<ScrapingErrorRetryConfigurationDto>();

            BillingCompany billingCompany = billingCompanyRepository.GetBillingCompanyById(billingCompanyId);

            foreach (ScrapingErrorRetryConfiguration retryConfiguration in billingCompany.ScrapingErrorRetryConfigurations)
            {
                billingCompanyDtos.Add(MapcrapingErrorRetryConfigurationTocrapingErrorRetryConfigurationDto(retryConfiguration));
            }

            return billingCompanyDtos;
        }
    }
}