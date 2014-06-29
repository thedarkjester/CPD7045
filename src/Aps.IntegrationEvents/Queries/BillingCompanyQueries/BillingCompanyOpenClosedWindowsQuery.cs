using System;
using System.Linq;
using System.Collections.Generic;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;

namespace Aps.Integration.Queries.BillingCompanyQueries
{
    public class BillingCompanyOpenClosedWindowsQuery
    {
        private readonly IBillingCompanyRepository billingCompanyRepository;

        public BillingCompanyOpenClosedWindowsQuery(IBillingCompanyRepository billingCompanyRepository)
        {
            this.billingCompanyRepository = billingCompanyRepository;
        }

        private OpenClosedWindowDto MapOpenClosedWindowAggregateToOpenClosedWindowDto(OpenClosedScrapingWindow openClosedScrapingWindow)
        {
            var openClosedWindowDto = new OpenClosedWindowDto
                {
                    StartDate = openClosedScrapingWindow.StartDate,
                    EndDate = openClosedScrapingWindow.EndDate,
                    ConcurrentScrapingLimit = openClosedScrapingWindow.ConcurrentScrapingLimit,
                    IsOpen = openClosedScrapingWindow.IsOpen
                };

            return openClosedWindowDto;
        }

        public IEnumerable<OpenClosedWindowDto> GetAllOpenClosedWindows(Guid billingCompanyId)
        {
            List<OpenClosedWindowDto> billingCompanyDtos = new List<OpenClosedWindowDto>();

            BillingCompany billingCompany = billingCompanyRepository.GetBillingCompanyById(billingCompanyId);

            foreach (OpenClosedScrapingWindow openClosedWindow in billingCompany.OpenClosedScrapingWindows)
            {
                billingCompanyDtos.Add(MapOpenClosedWindowAggregateToOpenClosedWindowDto(openClosedWindow));
            }

            return billingCompanyDtos;
        }

        public OpenClosedWindowDto GetCurrentOpenClosedWindow(Guid billingCompanyId)
        {
            DateTime currentTime = DateTime.UtcNow;

            BillingCompany billingCompany = billingCompanyRepository.GetBillingCompanyById(billingCompanyId);

            OpenClosedScrapingWindow openClosedScrapingWindow = billingCompany.OpenClosedScrapingWindows.FirstOrDefault(x => x.StartDate <= currentTime && x.EndDate <= currentTime);

            return MapOpenClosedWindowAggregateToOpenClosedWindowDto(openClosedScrapingWindow);
        }
    }
}