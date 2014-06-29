using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;
using System;

namespace Aps.Integration.Queries.BillingCompanyQueries
{
    public class BillingCompanyCrossCheckEnabledByIdQuery
    {
        private readonly IBillingCompanyRepository billingCompanyRepository;

        public BillingCompanyCrossCheckEnabledByIdQuery(IBillingCompanyRepository billingCompanyRepository)
        {
            this.billingCompanyRepository = billingCompanyRepository;
        }

        public BillingCompanyCrossCheckDto GetBillingCompanyCrossCheckEnabledById(Guid id)
        {
            BillingCompany billingCompany = billingCompanyRepository.GetBillingCompanyById(id);
          
            if (billingCompany == null)
            {
                return null;
            }

            BillingCompanyCrossCheckDto dto = MapBillingCompanyAggregateToBillingCompanyDto(billingCompany);

            return dto;
        }

        private BillingCompanyCrossCheckDto MapBillingCompanyAggregateToBillingCompanyDto(BillingCompany billingCompany)
        {
            var billingCompanyCrossCheckDto = new BillingCompanyCrossCheckDto
                {
                    crossCheckScrapeEnabled = billingCompany.CrossCheckScrapeEnabled.CrossCheckScrapeEnabled
                };

            return billingCompanyCrossCheckDto;
        }
    }
}
