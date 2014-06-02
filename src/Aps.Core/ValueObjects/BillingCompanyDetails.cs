using System;
using Seterlund.CodeGuard;

namespace Aps.Core.ValueObjects
{
    public class BillingCompanyDetails
    {
        private readonly Guid billingCompanyId;
        private readonly string companyName;
        public string CompanyName
        {
            get { return companyName; }
        }

        public Guid BillingCompanyId
        {
            get { return billingCompanyId; }
        }

        public BillingCompanyDetails(Guid billingCompanyId, string companyName)
        {
            Guard.That(billingCompanyId).IsNotEmpty();
            Guard.That(companyName).IsNotEmpty();
            Guard.That(companyName).IsNotNull();

            this.billingCompanyId = billingCompanyId;
            this.companyName = companyName;
        }

      
    }
}