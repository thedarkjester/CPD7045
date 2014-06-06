using System;
using System.Collections.Generic;
using System.Linq;
using Aps.AccountStatements;
using Aps.AccountStatements.Entities;
using Caliburn.Micro;

namespace Aps.Fakes
{
    public class AccountStatementRepositoryFake : IAccountStatementRepository
    {
        public AccountStatement GetAccountStatementById(Guid id)
        {
            throw new NotImplementedException();
        }

        private readonly List<AccountStatement> accountStatements;

        private readonly IEventAggregator eventAggregator;

        public AccountStatementRepositoryFake(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.accountStatements = new List<AccountStatement>();
        }

        public void StoreBillingCompany(AccountStatement accountStatement)
        {
            // validate Ids?
            this.accountStatements.Add(accountStatement);
        }


        public AccountStatement GetBillingCompanyById(Guid id)
        {
            return this.accountStatements.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<AccountStatement> GetAllAccountStatements()
        {
            return accountStatements;
        }
    }
}