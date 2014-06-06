using Aps.BillingCompanies;
using Aps.Core.Scrappers;
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
   
    public class ScrapeSessionAgent
    {
        private readonly IEventAggregator eventAggregator;
        private readonly CustomerRepositoryFake customerRepositoryFake;
        private readonly BillingCompanyRepositoryFake billingCompanyRepositoryFake;
        private readonly EventIntegrationService messageSendAndReceiver;
        private readonly IIndex<ScrapeSessionType, Scrapper> scrappers;
        public ScrapeSessionAgent(IEventAggregator eventAggregator, CustomerRepositoryFake customerRepositoryFake, BillingCompanyRepositoryFake billingCompanyRepositoryFake, EventIntegrationService messageSendAndReceiver, IIndex<ScrapeSessionType, Scrapper> scrappers)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            this.customerRepositoryFake = customerRepositoryFake;
            this.billingCompanyRepositoryFake = billingCompanyRepositoryFake;
            this.messageSendAndReceiver = messageSendAndReceiver;
            this.scrappers = scrappers;
        }

        public void InitiateScrape(Guid billingCompanyId, Guid customerId, ScrapeSessionType scrapeSessionType)
        {
            Scrapper scrapper = scrappers[scrapeSessionType];

            //get user credentials for billing company 

           Task.Run(() => scrapper.Scrape()/*(billingCompanyId, customerId)*/);
        }
    }
}
