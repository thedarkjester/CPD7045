using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Integration.Events
{
    [Serializable]
    public class CrossCheckSessionCompletedWithErrors
    {
        Guid crossCheckSessionId;
        Guid customerId;
        Guid billingCompanyId;
        DateTime dateCreated;
        string accountNumber;
        string error;

        public Guid CrossCheckSessionId { get { return crossCheckSessionId; } }
        public Guid CustomerId { get { return customerId; } }
        public Guid BillingCompanyId { get { return billingCompanyId; } }
        public DateTime DateCreated { get { return dateCreated; } }
        public string AccountNumber { get { return accountNumber; } }
        public string Error { get { return error; } }
        public CrossCheckSessionCompletedWithErrors(Guid crossCheckSessionId, Guid customerId, Guid billingCompanyId, string accountNumber, string error)
        {
            this.crossCheckSessionId = crossCheckSessionId;
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            this.accountNumber = accountNumber;
            this.error = error;
            dateCreated = DateTime.Now;
        }
    }
}
