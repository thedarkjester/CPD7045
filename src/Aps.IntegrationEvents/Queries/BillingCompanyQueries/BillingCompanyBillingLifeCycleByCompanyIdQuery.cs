using System;
using Aps.BillingCompanies;
using Aps.IntegrationEvents.Queries.BillingCompanyQueries.Dtos;

namespace Aps.IntegrationEvents.Queries.BillingCompanyQueries
{
    public class BillingCompanyBillingLifeCycleByCompanyIdQuery
    {
        private readonly BillingCompanyRepositoryFake billingCompanyRepository;

        public BillingCompanyBillingLifeCycleByCompanyIdQuery(BillingCompanyRepositoryFake billingCompanyRepository)
        {
            this.billingCompanyRepository = billingCompanyRepository;
        }

        public BillingCompanyBillingLifeCycleDto GetBillingCompanyBillingLifeCycleByCompanyId(Guid id)
        {
            BillingCompanies.Aggregates.BillingCompany billingCompany = billingCompanyRepository.GetBillingCompanyById(id);

            if (billingCompany == null)
            {
                return null;
            }

            BillingCompanyBillingLifeCycleDto dto = MapBillingCompanyAggregateToBillingCompanyBillingLifeCycleDto(billingCompany);

            return dto;
        }

        private BillingCompanyBillingLifeCycleDto MapBillingCompanyAggregateToBillingCompanyBillingLifeCycleDto(BillingCompanies.Aggregates.BillingCompany billingCompany)
        {
            var billingCompanyLifeCycleDto = new BillingCompanyBillingLifeCycleDto
                {
                    Id = billingCompany.Id,
                    Name = billingCompany.BillingCompanyName.Name,
                    DaysPerBillingCycle = billingCompany.BillingLifeCycle.DaysPerBillingCycle,
                    LeadTimeInterval = billingCompany.BillingLifeCycle.LeadTimeInterval,
                    RetryInterval = billingCompany.BillingLifeCycle.RetryInterval
                };

            return billingCompanyLifeCycleDto;
        }

    }
}