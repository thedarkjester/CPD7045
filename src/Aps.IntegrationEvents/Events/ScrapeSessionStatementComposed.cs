using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Integration.Events
{
    public class ScrapeSessionStatementComposed
    {
        Guid scrapeSessionId;
        Guid customerId;
        Guid billingCompanyId;
        DateTime dateCreated;
        Guid accountStatementId;
        DateTime statementDate;

        public Guid ScrapeSessionId { get { return scrapeSessionId; } }
        public Guid CustomerId { get { return customerId; } }
        public Guid BillingCompanyId { get { return billingCompanyId; } }
        public DateTime DateCreated { get { return dateCreated; } }
        public Guid AccountStatementId { get { return accountStatementId;  } }
        public DateTime StatementDate { get { return statementDate; } }

        public ScrapeSessionStatementComposed(Guid scrapeSessionId, Guid customerId, Guid billingComapnyId, Guid accountStatementId, DateTime statementDate)
        {
            this.scrapeSessionId = scrapeSessionId;
            this.customerId = customerId;
            this.billingCompanyId = billingComapnyId;
            this.accountStatementId = accountStatementId;
            this.statementDate = statementDate;
            dateCreated = DateTime.Now;
        }
    }
}
