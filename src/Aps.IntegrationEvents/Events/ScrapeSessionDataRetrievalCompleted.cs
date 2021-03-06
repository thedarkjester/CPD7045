﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Integration.Events
{
    [Serializable]
    public class ScrapeSessionDataRetrievalCompleted
    {

        Guid scrapeSessionId; 
        Guid customerId;
        Guid billingCompanyId;
        DateTime dateCreated;

        public Guid ScrapeSessionIdv { get { return scrapeSessionId; } }
        public Guid CustomerId { get { return customerId; } }
        public Guid BillingCompanyId { get { return billingCompanyId; } }
        public DateTime DateCreated { get { return dateCreated; } }

        public ScrapeSessionDataRetrievalCompleted(Guid scrapeSessionId, Guid customerId, Guid billingCompanyId)
        {
            this.scrapeSessionId = scrapeSessionId;
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            dateCreated = DateTime.Now;
        }
    }
}
