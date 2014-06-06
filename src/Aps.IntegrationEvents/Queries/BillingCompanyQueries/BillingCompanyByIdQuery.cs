using System;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;

namespace Aps.Integration.Queries.BillingCompanyQueries
{
    public class BillingCompanyByIdQuery
    {
        private readonly IBillingCompanyRepository billingCompanyRepository;

        public BillingCompanyByIdQuery(IBillingCompanyRepository billingCompanyRepository)
        {
            this.billingCompanyRepository = billingCompanyRepository;
        }

        public BillingCompanyDto GetBillingCompanyById(Guid id)
        {
            BillingCompany billingCompany = billingCompanyRepository.GetBillingCompanyById(id);
          
            if (billingCompany == null)
            {
                return null;
            }

            BillingCompanyDto dto = MapBillingCompanyAggregateToBillingCompanyDto(billingCompany);

            return dto;
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
    }
}