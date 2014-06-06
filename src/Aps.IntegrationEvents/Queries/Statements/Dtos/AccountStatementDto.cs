using System.Collections.Generic;
using Seterlund.CodeGuard;

namespace Aps.Integration.Queries.Statements.Dtos
{
    public class AccountStatementDto
    {
        private readonly List<AccountStatementTransactionDto> statementTransactions;
        private CustomerDetailsDto customerDetails;
        private BillingCompanyDetailsDto billingCompanyDetails;
        private StatementDateDto statementDate;

        public AccountStatementDto(CustomerDetailsDto customerDetails, BillingCompanyDetailsDto billingCompanyDetails,

            StatementDateDto statementDate, List<AccountStatementTransactionDto> statementTransactions)
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