using System;
using System.Collections.Generic;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;

namespace Aps.BillingCompanies
{
    public interface IBillingCompanyRepository
    {
        void StoreBillingCompany(BillingCompany billingCompany);
        BillingCompany GetBillingCompanyById(Guid id);
        IEnumerable<BillingCompany> GetAllBillingCompanies();
        void RemoveBillingCompanyById(Guid addedBillingCompanyId);
    }
}