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

        public Guid ScrapeSessionId { get { return scrapeSessionId; } }
        public Guid CustomerId { get { return customerId; } }
        public Guid BillingCompanyId { get { return billingCompanyId; } }
        public DateTime DateCreated { get { return dateCreated; } }
        Guid AccountStatementId { get; set; }
        DateTime StatementDate { get; set; }

        public ScrapeSessionStatementComposed(Guid scrapeSessionId, Guid customerId, Guid billingComapnyId, Guid accountStatementId, DateTime statementDate)
        {
            this.scrapeSessionId = scrapeSessionId;
            this.customerId = customerId;
            this.billingCompanyId = billingComapnyId;
            AccountStatementId = accountStatementId;
            StatementDate = statementDate;
            dateCreated = DateTime.Now;
        }
    }
}
