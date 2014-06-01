﻿using System;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;

namespace Aps.Integration.Queries.BillingCompanyQueries
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
            BillingCompany billingCompany = billingCompanyRepository.GetBillingCompanyById(id);

            if (billingCompany == null)
            {
                return null;
            }

            BillingCompanyBillingLifeCycleDto dto = MapBillingCompanyAggregateToBillingCompanyBillingLifeCycleDto(billingCompany);

            return dto;
        }

        private BillingCompanyBillingLifeCycleDto MapBillingCompanyAggregateToBillingCompanyBillingLifeCycleDto(BillingCompany billingCompany)
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