using System;
using System.Collections.Generic;
using Aps.AccountStatements.Entities;

namespace Aps.AccountStatements
{
    public interface IAccountStatementRepository
    {
        AccountStatement GetAccountStatementById(Guid id);
        void StoreAccountStatement(AccountStatement accountStatement);
        IEnumerable<AccountStatement> GetAllAccountStatements();
    }
}