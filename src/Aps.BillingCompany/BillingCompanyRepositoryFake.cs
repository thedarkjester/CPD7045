using System;
using System.Linq;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Caliburn.Micro;
using System.Collections.Generic;
using Seterlund.CodeGuard;

namespace Aps.BillingCompanies
{
    public class BillingCompanyRepositoryFake
    {
        private readonly List<BillingCompany> billingCompanies;

        private readonly IEventAggregator eventAggregator;

        public BillingCompanyRepositoryFake(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.billingCompanies = new List<BillingCompany>();
        }

        public void StoreBillingCompany(BillingCompany billingCompany)
        {
            // validate Ids?
            this.billingCompanies.Add(billingCompany);
        }

        public BillingCompany GetNewBillingCompany(BillingCompanyName billingCompanyName,BillingCompanyType billingCompanyType,BillingCompanyScrapingUrl billingCompanyScrapingUrl)
        {
            Guard.That(billingCompanyName).IsNotNull();
            Guard.That(billingCompanyType).IsNotNull();
            Guard.That(billingCompanyScrapingUrl).IsNotNull();

            return new BillingCompany(eventAggregator, billingCompanyName, billingCompanyType, billingCompanyScrapingUrl);
        }

        public BillingCompany GetBillingCompanyById(Guid id)
        {
            return this.billingCompanies.FirstOrDefault(x => x.Id == id);
        }
    }
}