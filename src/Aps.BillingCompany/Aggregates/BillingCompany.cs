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
        private BillingCompanyType billingCompanyType;
        private BillingCompanyScrapingUrl billingCompanyScrapingUrl;

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

        public BillingCompanyType BillingCompanyType
        {
            get { return this.billingCompanyType; }
        }

        public BillingCompanyScrapingUrl BillingCompanyScrapingUrl
        {
            get { return this.billingCompanyScrapingUrl; }
        }

        public IEventAggregator EventAggregator { get; set; }

        public BillingCompany(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.openClosedWindows = new List<OpenClosedWindow>();
            this.scrapingErrorRetryConfigurations = new List<ScrapingErrorRetryConfiguration>();
        }

        public void SetBillingLifeCycle(BillingLifeCycle lifeCycle)
        {
            // validation of action

            this.billingLifeCycle = lifeCycle;
        }

        public void SetScrapingLoadManagementConfiguration(ScrapingLoadManagementConfiguration loadManagementConfiguration)
        {
            // validation of action

            this.scrapingLoadManagementConfiguration = loadManagementConfiguration;
        }

        public void SetBillingCompanyType(BillingCompanyType companyType)
        {
            // validation of action

            this.billingCompanyType = companyType;
        }

        public void SetBillingCompanyUrl(BillingCompanyScrapingUrl scrapingUrl)
        {
            // validation of action
            this.billingCompanyScrapingUrl = scrapingUrl;
        }

        public void AddOpenClosedWindow(OpenClosedWindow openClosedWindow)
        {
            // validation of action

            this.openClosedWindows.Add(openClosedWindow);
        }

        public void RemoveOpenClosedWindow(OpenClosedWindow openClosedWindow)
        {
            // validation of action

            this.openClosedWindows.Remove(openClosedWindow);
        }

        public void AddScrapingErrorRetryConfiguration(ScrapingErrorRetryConfiguration scrapingErrorRetryConfiguration)
        {
            // validation of action

            this.scrapingErrorRetryConfigurations.Add(scrapingErrorRetryConfiguration);
        }

        public void RemoveScrapingErrorRetryConfiguration(ScrapingErrorRetryConfiguration scrapingErrorRetryConfiguration)
        {
            // validation of action

            this.scrapingErrorRetryConfigurations.Remove(scrapingErrorRetryConfiguration);
        }
    }
}
