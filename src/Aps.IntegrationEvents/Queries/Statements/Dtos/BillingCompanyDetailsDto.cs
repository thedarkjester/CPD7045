using System;
using Seterlund.CodeGuard;

namespace Aps.Integration.Queries.Statements.Dtos
{
    public class BillingCompanyDetailsDto
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

        public BillingCompanyDetailsDto(Guid billingCompanyId, string companyName)
        {
            Guard.That(billingCompanyId).IsNotEmpty();
            Guard.That(companyName).IsNotEmpty();
            Guard.That(companyName).IsNotNull();

            this.billingCompanyId = billingCompanyId;
            this.companyName = companyName;
        }
    }
}