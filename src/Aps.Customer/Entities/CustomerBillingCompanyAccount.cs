using System;
using Seterlund.CodeGuard;
using Aps.Customers.ValueObjects;


namespace Aps.Customers.Entities
{
    public class CustomerBillingCompanyAccount
    {
        public readonly Guid billingCompanyId;
        public readonly string billingCompanyUsername;
        public readonly string billingCompanyPassword;
        public string billingCompanyStatus;
        public string billingCompanyAccountNumber;
        public readonly int billingCompanyPIN;
        public readonly DateTime dateBillingCompanyAdded;
        public CustomerStatement customerStatement;

        protected CustomerBillingCompanyAccount()
        {

        }

        public CustomerBillingCompanyAccount(Guid billingCompanyId, string billingCompanyUsername, string billingCompanyPassword, string billingCompanyStatus,
                                             string billingCompanyAccountNumber, int billingCompanyPIN, DateTime dateBillingCompanyAdded)
        {
            Guard.That(billingCompanyId).IsNotEmpty();
            Guard.That(billingCompanyUsername).IsNotNullOrEmpty();
            Guard.That(billingCompanyPassword).IsNotNullOrEmpty();
            Guard.That(billingCompanyStatus).IsNotNullOrEmpty();
            Guard.That(billingCompanyAccountNumber).IsNotNullOrEmpty();
            Guard.That(billingCompanyPIN).IsGreaterThan(0);
            Guard.That(dateBillingCompanyAdded).IsTrue(date => date >= DateTime.Now, "dateBillingCompanyAdded cannot be in the past");
            
            this.billingCompanyId = billingCompanyId;
            this.billingCompanyUsername = billingCompanyUsername;
            this.billingCompanyPassword = billingCompanyPassword;
            this.billingCompanyStatus = billingCompanyStatus;
            this.billingCompanyAccountNumber = billingCompanyAccountNumber;
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