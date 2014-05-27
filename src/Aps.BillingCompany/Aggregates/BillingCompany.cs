using System;
using System.Collections.Generic;
using System.Linq;
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
        private BillingCompanyName billingCompanyName;

        public IEnumerable<OpenClosedWindow> OpenClosedWindows { get { return openClosedWindows; } }
        public IEnumerable<ScrapingErrorRetryConfiguration> ScrapingErrorRetryConfigurations { get { return scrapingErrorRetryConfigurations; } }

        public BillingLifeCycle BillingLifeCycle
        {
            get { return this.billingLifeCycle; }
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

        public BillingCompany(IEventAggregator eventAggregator, BillingCompanyName companyName, BillingCompanyType companyType, BillingCompanyScrapingUrl companyScrapingUrl)
        {
            this.billingCompanyName = companyName;
            this.billingCompanyType = companyType;
            this.billingCompanyScrapingUrl = companyScrapingUrl;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

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

        public void SetBillingCompanyName(BillingCompanyName companyName)
        {
            // validation of action

            this.billingCompanyName = companyName;
        }

        public void SetBillingCompanyUrl(BillingCompanyScrapingUrl scrapingUrl)
        {
            // validation of action
            this.billingCompanyScrapingUrl = scrapingUrl;
        }

        public void AddOpenClosedWindow(OpenClosedWindow openClosedWindow)
        {
            GuardAgainstOverlappingOpenClosedWindows(openClosedWindow);
            // validation of action

            this.openClosedWindows.Add(openClosedWindow);
        }

        private void GuardAgainstOverlappingOpenClosedWindows(OpenClosedWindow openClosedWindow)
        {
            foreach (var existingWindow in openClosedWindows)
            {
                if (openClosedWindow.StartDate.Between(existingWindow.StartDate, existingWindow.EndDate))
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (openClosedWindow.EndDate.Between(existingWindow.StartDate, existingWindow.EndDate))
                {
                    throw new ArgumentOutOfRangeException();
                }  
            }
        }

        public void RemoveOpenClosedWindow(OpenClosedWindow openClosedWindow)
        {
            // validation of action

            this.openClosedWindows.Remove(openClosedWindow);
        }

        public void AddScrapingErrorRetryConfiguration(ScrapingErrorRetryConfiguration scrapingErrorRetryConfiguration)
        {
            // validation of action
            if (scrapingErrorRetryConfigurations.Any(x => x.ResponseCode == scrapingErrorRetryConfiguration.ResponseCode))
            {
                throw new InvalidOperationException("Duplicate Error Code Configuration Exists");
            }

            this.scrapingErrorRetryConfigurations.Add(scrapingErrorRetryConfiguration);
        }

        public void RemoveScrapingErrorRetryConfiguration(ScrapingErrorRetryConfiguration scrapingErrorRetryConfiguration)
        {
            // validation of action
            if (scrapingErrorRetryConfigurations.Contains(scrapingErrorRetryConfiguration))
            {
                this.scrapingErrorRetryConfigurations.Remove(scrapingErrorRetryConfiguration);
            }
        }
    }

    public static class TestExtensions
    {
        public static bool Between(this DateTime input, DateTime date1, DateTime date2)
        {
            return (input > date1 && input < date2);
        }
    }
}
