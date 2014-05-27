using System;
using System.Collections;
using System.Collections.Generic;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;

namespace Aps.Integration.Queries.BillingCompanyQueries
{
    public class AllBillingCompaniesQuery
    {
        private readonly BillingCompanyRepositoryFake billingCompanyRepository;

        public AllBillingCompaniesQuery(BillingCompanyRepositoryFake billingCompanyRepository)
        {
            this.billingCompanyRepository = billingCompanyRepository;
        }
     
        private BillingCompanyDto MapBillingCompanyAggregateToBillingCompanyDto(BillingCompany billingCompany)
        {
            var billingCompanyDto = new BillingCompanyDto
                {
                    Id = billingCompany.Id,
                    Name = billingCompany.BillingCompanyName.Name
                };

            return billingCompanyDto;
        }

        public IEnumerable<BillingCompanyDto> GetAllBillingCompanies()
        {
            List<BillingCompanyDto> billingCompanyDtos = new List<BillingCompanyDto>();

            IEnumerable<BillingCompany> billingCompanies = billingCompanyRepository.GetAllBillingCompanies();

            foreach (BillingCompany billingCompany in billingCompanies)
            {
                billingCompanyDtos.Add(MapBillingCompanyAggregateToBillingCompanyDto(billingCompany));
            }

            return billingCompanyDtos;
        }
    }
}