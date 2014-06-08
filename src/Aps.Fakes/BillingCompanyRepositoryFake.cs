using System;
using System.Linq;
using System.Collections.Generic;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Caliburn.Micro;
using Seterlund.CodeGuard;

namespace Aps.Fakes
{
    public class BillingCompanyRepositoryFake : IBillingCompanyRepository
    {
        private readonly List<BillingCompany> billingCompanies;

        private readonly IEventAggregator eventAggregator;
        private readonly BillingCompanyCreator billingCompanyCreator;

        public BillingCompanyRepositoryFake(IEventAggregator eventAggregator, BillingCompanyCreator billingCompanyCreator)
        {
            this.eventAggregator = eventAggregator;
            this.billingCompanyCreator = billingCompanyCreator;
            this.billingCompanies = new List<BillingCompany>();
        }

        public void StoreBillingCompany(BillingCompany billingCompany)
        {
            // validate Ids?
            this.billingCompanies.Add(billingCompany);
        }

        public BillingCompany BuildNewBillingCompany(BillingCompanyName billingCompanyName, BillingCompanyType billingCompanyType, BillingCompanyScrapingUrl billingCompanyScrapingUrl, bool crossCheckScrapeEnabled = false)
        {
            Guard.That(billingCompanyName).IsNotNull();
            Guard.That(billingCompanyType).IsNotNull();
            Guard.That(billingCompanyScrapingUrl).IsNotNull();

            return this.billingCompanyCreator.GetNewBillingCompany(billingCompanyName, billingCompanyType, billingCompanyScrapingUrl, crossCheckScrapeEnabled);
        }

        public BillingCompany GetBillingCompanyById(Guid id)
        {
            return this.billingCompanies.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<BillingCompany> GetAllBillingCompanies()
        {
            return billingCompanies;
        }

        public void RemoveBillingCompanyById(Guid billingCompanyId)
        {
            var billingCompany = billingCompanies.FirstOrDefault(company => company.Id == billingCompanyId);

            if (billingCompany == null)
            {
                this.billingCompanies.Remove(billingCompany);
            }
        }
    }
}