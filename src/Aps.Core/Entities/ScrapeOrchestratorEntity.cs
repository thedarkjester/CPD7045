using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.Entities
{
    public class ScrapeOrchestratorEntity
    {
        public Guid QueueId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid BillingCompanyId { get; private set; }
        public string Url { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string AccountNumber { get; private set; }
        public int Pin { get; private set; }
        public ScrapeOrchestratorEntity(Guid queueId, Guid customerId, Guid billingCompanyId, string url, string username, string password, int pin, string accountNumber)
        {
            QueueId = queueId;
            CustomerId = customerId;
            BillingCompanyId = billingCompanyId;
            Url = url;
            Username = username;
            Password = password;
            Pin = pin;
            AccountNumber = accountNumber;
        }
    }
}
