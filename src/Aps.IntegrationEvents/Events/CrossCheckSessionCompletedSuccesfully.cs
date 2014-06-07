using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Integration.Events
{
    public class CrossCheckSessionCompletedSuccesfully
    {
        Guid crossCheckSessionId;
        Guid customerId;
        Guid billingCompanyId;
        DateTime dateCreated;
        string accountNumber;

        public Guid CrossCheckSessionId { get { return crossCheckSessionId; } }
        public Guid CustomerId { get { return customerId; } }
        public Guid BillingCompanyId { get { return billingCompanyId; } }
        public DateTime DateCreated { get { return dateCreated; } }
        public string AccountNumber { get { return accountNumber; } }
        public CrossCheckSessionCompletedSuccesfully(Guid crossCheckSessionId, Guid customerId, Guid billingCompanyId, string accountNumber)
        {
            this.crossCheckSessionId = crossCheckSessionId;
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            this.accountNumber = accountNumber;
            dateCreated = DateTime.Now;
        }
    }
}
