using System.Collections.Generic;
using Aps.AccountStatements.ValueObjects;
using Aps.DomainBase;
using Seterlund.CodeGuard;

namespace Aps.AccountStatements.Entities
{
    public class AccountStatement : Entity
    {
        private readonly List<AccountStatementTransaction> statementTransactions;
        private CustomerDetails customerDetails;
        private BillingCompanyDetails billingCompanyDetails;
        private StatementDate statementDate;

        public List<AccountStatementTransaction> StatementTransactions
        {
            get { return statementTransactions; }
        }

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
        }

        public BillingCompanyDetails BillingCompanyDetails
        {
            get { return billingCompanyDetails; }
        }

        public StatementDate StatementDate
        {
            get { return statementDate; }
        }

        public AccountStatement(CustomerDetails customerDetails, BillingCompanyDetails billingCompanyDetails,
            StatementDate statementDate, List<AccountStatementTransaction> statementTransactions)
        {
            Guard.That(statementTransactions).IsNotNull();
            Guard.That(statementTransactions).IsTrue(x => x.Count > 0, "Count is == 0");
            Guard.That(customerDetails).IsNotNull();
            Guard.That(billingCompanyDetails).IsNotNull();
            Guard.That(statementDate).IsNotNull();

            this.customerDetails = customerDetails;
            this.billingCompanyDetails = billingCompanyDetails;
            this.statementDate = statementDate;
            this.statementTransactions = statementTransactions;
        }
    }
}