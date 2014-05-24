using Aps.BillingCompanies.Aggregates;

namespace Aps.BillingCompanies
{
    public class BillingCompanyRepository
    {
        public BillingCompany GetNewBillingCompany()
        {
            return new BillingCompany();
        }
    }
}