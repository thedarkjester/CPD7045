using Seterlund.CodeGuard;
using System;

namespace Aps.Integration.Events
{
    [Serializable]
    public class AccountStatementGenerated
    {
        public Guid AccountStatementId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime StatementDate { get; set; }

        public AccountStatementGenerated(Guid accountStatementId, Guid customerId, DateTime statementDate)
        {
            Guard.That(accountStatementId).IsNotEmpty();
            Guard.That(customerId).IsNotEmpty();
            Guard.That(statementDate).IsTrue(date => date >= DateTime.Now, "statementDate cannot be earlier than now");

            this.StatementDate = statementDate;
            this.CustomerId = customerId;
            this.StatementDate = statementDate;
        }
    }
}