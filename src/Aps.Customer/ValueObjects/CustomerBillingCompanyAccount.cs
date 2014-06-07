using System;
using Seterlund.CodeGuard;

namespace Aps.Customers.ValueObjects
{
    public class CustomerBillingCompanyAccount
    {
        private readonly Guid billingCompanyId;
        private readonly string billingCompanyUsername;
        private readonly string billingCompanyPassword;
        private string billingCompanyStatus;
        private readonly int billingCompanyPIN;
        private readonly DateTime dateBillingCompanyAdded;

        protected CustomerBillingCompanyAccount()
        {

        }

        public CustomerBillingCompanyAccount(Guid billingCompanyId, string billingCompanyUsername, string billingCompanyPassword, string billingCompanyStatus,
                                             int billingCompanyPIN, DateTime dateBillingCompanyAdded)
        {
            Guard.That(billingCompanyId).IsNotEmpty();
            Guard.That(billingCompanyUsername).IsNotNullOrEmpty();
            Guard.That(billingCompanyPassword).IsNotNullOrEmpty();
            Guard.That(billingCompanyStatus).IsNotNullOrEmpty();
            Guard.That(billingCompanyPIN).IsGreaterThan(0);
            Guard.That(dateBillingCompanyAdded).IsTrue(date => date >= DateTime.Now, "dateBillingCompanyAdded cannot be in the past");
            
            this.billingCompanyId = billingCompanyId;
            this.billingCompanyUsername = billingCompanyUsername;
            this.billingCompanyPassword = billingCompanyPassword;
            this.billingCompanyStatus = billingCompanyStatus;
            this.billingCompanyPIN = billingCompanyPIN;
            this.dateBillingCompanyAdded = dateBillingCompanyAdded;
        }

        public Guid getBillingCompanyId()
        {
            return billingCompanyId;
        }

        public void ChangeCustomerBillingAccountStatus(string status)
        {
            this.billingCompanyStatus = status;
        }

    }
}