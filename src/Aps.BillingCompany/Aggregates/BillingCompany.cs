using System.Collections.Generic;
using Aps.BillingCompanies.ValueObjects;
using Aps.DomainBase;
using Caliburn.Micro;

namespace Aps.BillingCompanies.Aggregates
{
    public class BillingCompany : Aggregate
    {
        private readonly IEventAggregator eventAggregator;
        private readonly List<OpenClosedWindow> openClosedWindows;
        private readonly List<ScrapingErrorRetryConfiguration> scrapingErrorRetryConfigurations;
     
        private BillingLifeCycle billingLifeCycle;
        private ScrapingLoadManagementConfiguration scrapingLoadManagementConfiguration;
        private ScrapingErrorRetryConfiguration scrapingErrorRetryConfiguration;
        private BillingCompanyType billingCompanyType;

        public IEnumerable<OpenClosedWindow> OpenClosedWindows { get { return openClosedWindows; } }
        public IEnumerable<ScrapingErrorRetryConfiguration> ScrapingErrorRetryConfigurations { get { return scrapingErrorRetryConfigurations; } }
       
        public BillingLifeCycle BillingLifeCycle
        {
            get { return this.billingLifeCycle; }
        }

        public ScrapingLoadManagementConfiguration ScrapingLoadManagementConfiguration
        {
            get { return this.scrapingLoadManagementConfiguration; }
        }

        public ScrapingErrorRetryConfiguration ScrapingErrorRetryConfiguration
        {
            get { return this.scrapingErrorRetryConfiguration; }
        }

        public BillingCompanyType BillingCompanyType
        {
            get { return this.billingCompanyType; }
        }

        public IEventAggregator EventAggregator { get; set; }
       
        public BillingCompany(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.openClosedWindows = new List<OpenClosedWindow>();
            this.scrapingErrorRetryConfigurations = new List<ScrapingErrorRetryConfiguration>();
        }
    }
}
