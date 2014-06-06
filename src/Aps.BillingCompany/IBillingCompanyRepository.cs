using System;
using System.Collections.Generic;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;

namespace Aps.BillingCompanies
{
    public interface IBillingCompanyRepository
    {
        void StoreBillingCompany(BillingCompany billingCompany);
        BillingCompany BuildNewBillingCompany(BillingCompanyName billingCompanyName, BillingCompanyType billingCompanyType, BillingCompanyScrapingUrl billingCompanyScrapingUrl, bool crossCheckScrapeEnabled = false);
        BillingCompany GetBillingCompanyById(Guid id);
        IEnumerable<BillingCompany> GetAllBillingCompanies();
    }
}