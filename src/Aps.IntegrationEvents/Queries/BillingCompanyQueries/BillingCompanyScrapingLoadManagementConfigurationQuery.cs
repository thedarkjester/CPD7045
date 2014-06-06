using System;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;

namespace Aps.Integration.Queries.BillingCompanyQueries
{
    public class BillingCompanyScrapingLoadManagementConfigurationQuery
    {
        private readonly IBillingCompanyRepository billingCompanyRepository;

        public BillingCompanyScrapingLoadManagementConfigurationQuery(IBillingCompanyRepository billingCompanyRepository)
        {
            this.billingCompanyRepository = billingCompanyRepository;
        }

        public BillingCompanyScrapingLoadManagementConfigurationDto GetBillingCompanyScrapingLoadManagementConfigurationById(Guid id)
        {
            BillingCompany billingCompany = billingCompanyRepository.GetBillingCompanyById(id);

            if (billingCompany == null)
            {
                return null;
            }

            BillingCompanyScrapingLoadManagementConfigurationDto dto = MapBillingCompanyScrapingLoadManagementConfigurationToBillingCompanyScrapingUrlDto(billingCompany);

            return dto;
        }

        private BillingCompanyScrapingLoadManagementConfigurationDto MapBillingCompanyScrapingLoadManagementConfigurationToBillingCompanyScrapingUrlDto(BillingCompany billingCompany)
        {
            var billingCompanyDto = new BillingCompanyScrapingLoadManagementConfigurationDto
                {
                    ConcurrentScrapes = billingCompany.ScrapingLoadManagementConfiguration.ConcurrentScrapes
                };

            return billingCompanyDto;
        }
    }
}