using Aps.BillingCompanies;
using Aps.Scheduling.ApplicationService.InternalEvents;
using Aps.Customers;
using Aps.Integration;
using Aps.Integration.EnumTypes;
using Aps.Scheduling.ApplicationService.ScrapeOrchestrators;
using Autofac.Features.Indexed;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aps.Scraping;
using Aps.Integration.Queries.CustomerQueries.Dtos;
using Aps.Scheduling.ApplicationService.Entities;

namespace Aps.Scheduling.ApplicationService
{
   
    public class ScrapeSessionInitiator
    {
        private readonly Aps.Integration.Queries.CustomerQueries.Dtos.CustomerBillingCompanyAccountsById customerBillingCompanyAccountsById;
        private readonly IIndex<ScrapeSessionTypes, ScrapeOrchestrator> scrapeOrchestrators;
        public ScrapeSessionInitiator(Aps.Integration.Queries.CustomerQueries.Dtos.CustomerBillingCompanyAccountsById customerBillingCompanyAccountsById, IIndex<ScrapeSessionTypes, ScrapeOrchestrator> scrapeOrchestrators)
        {
            this.customerBillingCompanyAccountsById = customerBillingCompanyAccountsById;
            this.scrapeOrchestrators = scrapeOrchestrators;
        }

        public void InitiateNewScrapeSession(ScrapingObject scrapingObject)
        {
            ScrapeOrchestrator scrapeOrchestrator = scrapeOrchestrators[scrapingObject.scrapeSessionTypes];

            CustomerBillingCompanyAccountDto customerBillingCompanyAccountDto = customerBillingCompanyAccountsById.GetCustomerBillingCompanyAccountByCustomerIdAndBillingCompanyId(scrapingObject.customerId, scrapingObject.billingCompanyId);
            ScrapeOrchestratorEntity scrapeOrchestratorEntity = new ScrapeOrchestratorEntity(scrapingObject.queueId, scrapingObject.customerId, scrapingObject.billingCompanyId, string.Empty, customerBillingCompanyAccountDto.billingCompanyUsername, customerBillingCompanyAccountDto.billingCompanyPassword, customerBillingCompanyAccountDto.billingCompanyPIN, customerBillingCompanyAccountDto.billingCompanyAccountNumber);
            Task.Run(() => scrapeOrchestrator.Orchestrate(scrapeOrchestratorEntity));
        }
    }
}
