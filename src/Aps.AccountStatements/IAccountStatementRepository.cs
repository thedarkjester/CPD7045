using System;
using System.Collections.Generic;
using Aps.AccountStatements.Entities;

namespace Aps.AccountStatements
{
    public interface IAccountStatementRepository
    {
        AccountStatement GetAccountStatementById(Guid id);
        void StoreBillingCompany(AccountStatement accountStatement);
        AccountStatement GetBillingCompanyById(Guid id);
        IEnumerable<AccountStatement> GetAllAccountStatements();
        bool AccountStatementExistsForCustomer(Guid customerId, Guid billingCompanyId, DateTime statementDate);
    }
}