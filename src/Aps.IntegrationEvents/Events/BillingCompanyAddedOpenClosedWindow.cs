using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seterlund.CodeGuard;

namespace Aps.Integration
{
    [Serializable]
    public class BillingCompanyAddedOpenClosedWindow
    {

        public Guid BillingCompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOpen { get; set; }
        public int ConcurrentScrapingLimit { get; set; }

        private BillingCompanyAddedOpenClosedWindow()
        {

        }

        public BillingCompanyAddedOpenClosedWindow(DateTime startDate, DateTime endDate, bool isOpen, int concurrentScrapingLimit, Guid billingCompanyId)
        {
            Guard.That(billingCompanyId).IsNotEmpty();
            if (isOpen)
            {
                Guard.That(concurrentScrapingLimit).IsGreaterThan(0);
            }

            Guard.That(startDate).IsTrue(time =>  time >= DateTime.Now,"startdate cannot be in the past");
            Guard.That(endDate).IsTrue(time =>  time >= DateTime.Now,"enddate cannot be in the past");

            Guard.That(startDate).IsLessThan(endDate);

            BillingCompanyId = billingCompanyId;
            ConcurrentScrapingLimit = concurrentScrapingLimit;
            IsOpen = isOpen;
            EndDate = endDate;
            StartDate = startDate;
        }
    }
}
