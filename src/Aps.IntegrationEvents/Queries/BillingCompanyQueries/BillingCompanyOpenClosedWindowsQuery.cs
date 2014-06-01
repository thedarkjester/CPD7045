using System;
using System.Collections.Generic;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;

namespace Aps.Integration.Queries.BillingCompanyQueries
{
    public class BillingCompanyOpenClosedWindowsQuery
    {
        private readonly BillingCompanyRepositoryFake billingCompanyRepository;

        public BillingCompanyOpenClosedWindowsQuery(BillingCompanyRepositoryFake billingCompanyRepository)
        {
            this.billingCompanyRepository = billingCompanyRepository;
        }

        private OpenClosedWindowDto MapOpenClosedWindowAggregateToOpenClosedWindowDto(OpenClosedWindow openClosedWindow)
        {
            var openClosedWindowDto = new OpenClosedWindowDto
                {
                    StartDate = openClosedWindow.StartDate,
                    EndDate = openClosedWindow.EndDate,
                    ConcurrentScrapingLimit = openClosedWindow.ConcurrentScrapingLimit,
                    IsOpen = openClosedWindow.IsOpen
                };

            return openClosedWindowDto;
        }

        public IEnumerable<OpenClosedWindowDto> GetAllOpenClosedWindows(Guid billingCompanyId)
        {
            List<OpenClosedWindowDto> billingCompanyDtos = new List<OpenClosedWindowDto>();

            BillingCompany billingCompany = billingCompanyRepository.GetBillingCompanyById(billingCompanyId);

            foreach (OpenClosedWindow openClosedWindow in billingCompany.OpenClosedWindows)
            {
                billingCompanyDtos.Add(MapOpenClosedWindowAggregateToOpenClosedWindowDto(openClosedWindow));
            }

            return billingCompanyDtos;
        }
    }
}