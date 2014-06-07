using Aps.BillingCompanies;
using Aps.Core.InternalEvents;
using Aps.Core.ScrapeOrchestrators;
using Aps.Customers;
using Aps.Integration;
using Aps.Integration.EnumTypes;
using Autofac.Features.Indexed;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core
{
   
    public class ScrapeSessionInitiator
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ICustomerRepository customerRepository;
        private readonly IBillingCompanyRepository billingCompanyRepository;
        private readonly IIndex<ScrapeSessionTypes, ScrapeOrchestrator> scrapeOrchestrators;
        public ScrapeSessionInitiator(IEventAggregator eventAggregator, ICustomerRepository customerRepository, IBillingCompanyRepository billingCompanyRepository, IIndex<ScrapeSessionTypes, ScrapeOrchestrator> scrapeOrchestrators)
        {
            this.eventAggregator = eventAggregator;
            this.customerRepository = customerRepository;
            this.billingCompanyRepository = billingCompanyRepository;
            this.scrapeOrchestrators = scrapeOrchestrators;
        }

        public void InitiateNewScrapeSession(Guid billingCompanyId, Guid customerId, Guid queueId, ScrapeSessionTypes scrapeSessionType)
        {
            ScrapeOrchestrator scrapeOrchestrator = scrapeOrchestrators[scrapeSessionType];


            //get user credentials for billing company 
            eventAggregator.Publish(new ScrapeSessionStarted(queueId, customerId, billingCompanyId));
            Task.Run(() => scrapeOrchestrator.Orchestrate()/*(billingCompanyId, customerId)*/);
        }
    }
}
