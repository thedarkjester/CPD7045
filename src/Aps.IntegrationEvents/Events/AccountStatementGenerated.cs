using System;

namespace Aps.Integration.Events
{
    [Serializable]
    public class AccountStatementGenerated
    {
        Guid AccountStatementId { get; set; }
        Guid CustomerId { get; set; }
        DateTime StatementDate { get; set; }

        public AccountStatementGenerated()
        {
            
        }
    }
}