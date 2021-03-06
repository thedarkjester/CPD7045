﻿using Aps.BillingCompanies;
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
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;

namespace Aps.Scheduling.ApplicationService
{
   
    public class ScrapeSessionInitiator
    {
        readonly CustomerBillingCompanyAccountsById customerBillingCompanyAccountsById;
        readonly BillingCompanyScrapingUrlQuery billingCompanyScrapingUrlQuery;
        readonly IIndex<ScrapeSessionTypes, ScrapeOrchestrator> scrapeOrchestrators;
        public ScrapeSessionInitiator(CustomerBillingCompanyAccountsById customerBillingCompanyAccountsById, BillingCompanyScrapingUrlQuery billingCompanyScrapingUrlQuery, IIndex<ScrapeSessionTypes, ScrapeOrchestrator> scrapeOrchestrators)
        {
            this.customerBillingCompanyAccountsById = customerBillingCompanyAccountsById;
            this.billingCompanyScrapingUrlQuery = billingCompanyScrapingUrlQuery;
            this.scrapeOrchestrators = scrapeOrchestrators;
        }

        public void InitiateNewScrapeSession(ScrapingObject scrapingObject)
        {
            //ScrapeOrchestrator scrapeOrchestrator = scrapeOrchestrators[scrapingObject.scrapeSessionTypes];
            ScrapeOrchestrator scrapeOrchestrator = scrapeOrchestrators[scrapingObject.ScrapeSessionTypes];
            CustomerBillingCompanyAccountDto customerBillingCompanyAccountDto = customerBillingCompanyAccountsById.GetCustomerBillingCompanyAccountByCustomerIdAndBillingCompanyId(scrapingObject.CustomerId, scrapingObject.BillingCompanyId);
            BillingCompanyScrapingUrlDto billingCompanyScrapingUrlDto = billingCompanyScrapingUrlQuery.GetBillingCompanyScrapingUrlById(scrapingObject.BillingCompanyId);
            ScrapeOrchestratorEntity scrapeOrchestratorEntity = new ScrapeOrchestratorEntity(scrapingObject.QueueId, scrapingObject.CustomerId, scrapingObject.BillingCompanyId, billingCompanyScrapingUrlDto.Url, customerBillingCompanyAccountDto.billingCompanyUsername, customerBillingCompanyAccountDto.billingCompanyPassword, customerBillingCompanyAccountDto.billingCompanyPIN, customerBillingCompanyAccountDto.billingCompanyAccountNumber);
            Task.Run(() => scrapeOrchestrator.Orchestrate(scrapeOrchestratorEntity));
        }
    }
}
