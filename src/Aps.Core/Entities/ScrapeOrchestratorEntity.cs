using System;

namespace Aps.Scheduling.ApplicationService.Entities
{
    public class ScrapeOrchestratorEntity
    {
        public Guid QueueId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BillingCompanyId { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Pin { get; set; }
        public string AccountNumber { get; set; }

        public ScrapeOrchestratorEntity(Guid queueId, Guid customerId, Guid billingCompanyId, string url, string billingCompanyUsername, string password, int pin, string accountNumber)
        {
            QueueId = queueId;
            CustomerId = customerId;
            BillingCompanyId = billingCompanyId;
            Url = url;
            Username = billingCompanyUsername;
            Password = password;
            Pin = pin;
            AccountNumber = accountNumber;
            throw new NotImplementedException();
        }
    }
}