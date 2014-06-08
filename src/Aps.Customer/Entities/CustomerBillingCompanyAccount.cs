﻿using System;
using Seterlund.CodeGuard;
using Aps.Customers.ValueObjects;


namespace Aps.Customers.Entities
{
    public class CustomerBillingCompanyAccount
    {
        public readonly Guid BillingCompanyId;
        public readonly string BillingCompanyUsername;
        public readonly string BillingCompanyPassword;
        public string BillingCompanyStatus;
        public string BillingCompanyAccountNumber;
        public readonly int BillingCompanyPIN;
        public readonly DateTime DateBillingCompanyAdded;
        public CustomerStatement CustomerStatement;

        protected CustomerBillingCompanyAccount()
        {

        }

        public CustomerBillingCompanyAccount(Guid billingCompanyId, string billingCompanyUsername, string billingCompanyPassword, string billingCompanyStatus,
                                             string billingCompanyAccountNumber, int billingCompanyPin, DateTime dateBillingCompanyAdded)
        {
            Guard.That(billingCompanyId).IsNotEmpty();
            Guard.That(billingCompanyUsername).IsNotNullOrEmpty();
            Guard.That(billingCompanyPassword).IsNotNullOrEmpty();
            Guard.That(billingCompanyStatus).IsNotNullOrEmpty();
            Guard.That(billingCompanyAccountNumber).IsNotNullOrEmpty();
            Guard.That(billingCompanyPin).IsGreaterThan(0);
            Guard.That(dateBillingCompanyAdded).IsTrue(date => date >= DateTime.Now, "DateBillingCompanyAdded cannot be in the past");
            
            this.BillingCompanyId = billingCompanyId;
            this.BillingCompanyUsername = billingCompanyUsername;
            this.BillingCompanyPassword = billingCompanyPassword;
            this.BillingCompanyStatus = billingCompanyStatus;
            this.BillingCompanyAccountNumber = billingCompanyAccountNumber;
            this.BillingCompanyPIN = billingCompanyPin;
            this.DateBillingCompanyAdded = dateBillingCompanyAdded;
        }

       public void ChangeCustomerBillingAccountStatus(string status)
        {
            this.BillingCompanyStatus = status;
        }

    }
}