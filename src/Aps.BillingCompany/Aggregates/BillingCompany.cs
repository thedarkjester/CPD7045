using System;
using System.Collections.Generic;
using System.Linq;
using Aps.BillingCompanies.ValueObjects;
using Aps.DomainBase;
using Caliburn.Micro;
using Seterlund.CodeGuard;

namespace Aps.BillingCompanies.Aggregates
{
    public class BillingCompany : Entity
    {
        private readonly IEventAggregator eventAggregator;

        private readonly List<OpenClosedScrapingWindow> openClosedScrapingWindows;
        private readonly List<ScrapingErrorRetryConfiguration> scrapingErrorRetryConfigurations;
        
        private BillingLifeCycle billingLifeCycle;
        private ScrapingLoadManagementConfiguration scrapingLoadManagementConfiguration;
        private BillingCompanyType billingCompanyType;
        private BillingCompanyScrapingUrl billingCompanyScrapingUrl;
        private BillingCompanyName billingCompanyName;
        private BillingCompanyCrossCheckScrapeEnabled crossCheckScrapeEnabled;

        public IEnumerable<OpenClosedScrapingWindow> OpenClosedScrapingWindows { get { return openClosedScrapingWindows; } }
        public IEnumerable<ScrapingErrorRetryConfiguration> ScrapingErrorRetryConfigurations { get { return scrapingErrorRetryConfigurations; } }

        public BillingLifeCycle BillingLifeCycle
        {
            get { return this.billingLifeCycle; }
        }

        public BillingCompanyCrossCheckScrapeEnabled CrossCheckScrapeEnabled
        {
            get { return this.crossCheckScrapeEnabled; }
        }

        public BillingCompanyName BillingCompanyName
        {
            get { return this.billingCompanyName; }
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

        private BillingCompany()
        {

        }

        public BillingCompany(IEventAggregator eventAggregator, BillingCompanyName companyName, BillingCompanyType companyType, BillingCompanyScrapingUrl companyScrapingUrl, BillingCompanyCrossCheckScrapeEnabled crossCheckScrapeEnabled)
        {
            Guard.That(eventAggregator).IsNotNull();
            Guard.That(companyName).IsNotNull();
            Guard.That(companyType).IsNotNull();
            Guard.That(companyScrapingUrl).IsNotNull();
            Guard.That(crossCheckScrapeEnabled).IsNotNull();

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            this.billingCompanyName = companyName;
            this.billingCompanyType = companyType;
            this.billingCompanyScrapingUrl = companyScrapingUrl;
            this.crossCheckScrapeEnabled = crossCheckScrapeEnabled;
            this.openClosedScrapingWindows = new List<OpenClosedScrapingWindow>();
            this.scrapingErrorRetryConfigurations = new List<ScrapingErrorRetryConfiguration>();
        }

        public void SetBillingCompanyCrossCheckScrapeEnabled(BillingCompanyCrossCheckScrapeEnabled billingCompanyCrossCheckScrapeEnabled)
        {
            Guard.That(billingCompanyCrossCheckScrapeEnabled).IsNotNull();
            this.crossCheckScrapeEnabled = billingCompanyCrossCheckScrapeEnabled;
        }

        public void SetBillingLifeCycle(BillingLifeCycle lifeCycle)
        {
            Guard.That(lifeCycle).IsNotNull();
            this.billingLifeCycle = lifeCycle;
        }

        public void SetScrapingLoadManagementConfiguration(ScrapingLoadManagementConfiguration loadManagementConfiguration)
        {
            Guard.That(loadManagementConfiguration).IsNotNull();
            this.scrapingLoadManagementConfiguration = loadManagementConfiguration;
        }

        public void SetBillingCompanyType(BillingCompanyType companyType)
        {
            Guard.That(companyType).IsNotNull();
            this.billingCompanyType = companyType;
        }

        public void SetBillingCompanyName(BillingCompanyName companyName)
        {
            Guard.That(companyName).IsNotNull();
            this.billingCompanyName = companyName;
        }

        public void SetBillingCompanyUrl(BillingCompanyScrapingUrl scrapingUrl)
        {
            Guard.That(scrapingUrl).IsNotNull();
            this.billingCompanyScrapingUrl = scrapingUrl;
        }

        public void AddOpenClosedScrapingWindow
            (OpenClosedScrapingWindow openClosedScrapingWindow)
        {
            Guard.That(openClosedScrapingWindow).IsNotNull();

            GuardAgainstOverlappingOpenClosedWindows(openClosedScrapingWindow);
           
            this.openClosedScrapingWindows.Add(openClosedScrapingWindow);
        }

        private void GuardAgainstOverlappingOpenClosedWindows
            (OpenClosedScrapingWindow openClosedScrapingWindow)
        {
            foreach (var existingWindow in openClosedScrapingWindows)
            {
                if (openClosedScrapingWindow.StartDate.Between
                    (existingWindow.StartDate, existingWindow.EndDate))
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (openClosedScrapingWindow.EndDate.
                    Between(existingWindow.StartDate, existingWindow.EndDate))
                {
                    throw new ArgumentOutOfRangeException();
                }  
            }
        }

        public void RemoveOpenClosedWindow(OpenClosedScrapingWindow openClosedScrapingWindow)
        {
            // validation of action

            this.openClosedScrapingWindows.Remove(openClosedScrapingWindow);
        }

        public void AddScrapingErrorRetryConfiguration(ScrapingErrorRetryConfiguration scrapingErrorRetryConfiguration)
        {
            Guard.That(scrapingErrorRetryConfiguration).IsNotNull();

            if (scrapingErrorRetryConfigurations.Any(x => x.ResponseCode == scrapingErrorRetryConfiguration.ResponseCode))
            {
                throw new InvalidOperationException("Duplicate Error Code Configuration Exists");
            }

            this.scrapingErrorRetryConfigurations.Add(scrapingErrorRetryConfiguration);
        }

        public void RemoveScrapingErrorRetryConfiguration(ScrapingErrorRetryConfiguration scrapingErrorRetryConfiguration)
        {
            Guard.That(scrapingErrorRetryConfiguration).IsNotNull();

            if (scrapingErrorRetryConfigurations.Contains(scrapingErrorRetryConfiguration))
            {
                this.scrapingErrorRetryConfigurations.Remove(scrapingErrorRetryConfiguration);
            }
        }
    }
}
