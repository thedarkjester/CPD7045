using Aps.AccountStatements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.Validation
{
    public class DuplicateStatementValidator : IValidator
    {
        readonly IAccountStatementRepository accountStatementRepository;
        public DuplicateStatementValidator(IAccountStatementRepository accountStatementRepository)
        {
            this.accountStatementRepository = accountStatementRepository;
        }
        public void Validate(IList<InterpretedScrapeSessionDataPair> interpretedScrapeSessionDataPair, Guid customerId, Guid billingCompanyId)
        {

            object statementDate = interpretedScrapeSessionDataPair.Select(x => x.keyValuePair).FirstOrDefault(x => x.Key.ToLower() == "statement date").Value;

            if (statementDate == null || statementDate == "") return;

            bool isDuplicateStatement = accountStatementRepository.AccountStatementExistsForCustomer(customerId, billingCompanyId, Convert.ToDateTime(statementDate));

            if (isDuplicateStatement)
                throw new DuplicateStatementException();
        }
    }
}
