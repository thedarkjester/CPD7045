﻿using System;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;

namespace Aps.Integration.Queries.BillingCompanyQueries
{
    public class BillingCompanyScrapingUrlQuery
    {
        private readonly IBillingCompanyRepository billingCompanyRepository;

        public BillingCompanyScrapingUrlQuery(IBillingCompanyRepository billingCompanyRepository)
        {
            this.billingCompanyRepository = billingCompanyRepository;
        }

        public BillingCompanyScrapingUrlDto GetBillingCompanyScrapingUrlById(Guid id)
        {
            BillingCompany billingCompany = billingCompanyRepository.GetBillingCompanyById(id);

            if (billingCompany == null)
            {
                return null;
            }

            BillingCompanyScrapingUrlDto dto = MapBillingCompanyScrapingUrlToBillingCompanyScrapingUrlDto(billingCompany);

            return dto;
        }

        private BillingCompanyScrapingUrlDto MapBillingCompanyScrapingUrlToBillingCompanyScrapingUrlDto(BillingCompany billingCompany)
        {
            var billingCompanyDto = new BillingCompanyScrapingUrlDto
                {
                    Id = billingCompany.Id,
                    Url = billingCompany.BillingCompanyScrapingUrl.ScrapingUrl,
                    CrossCheckScrapeEnabled = billingCompany.CrossCheckScrapeEnabled.CrossCheckScrapeEnabled
                };

            return billingCompanyDto;
        }
    }
}