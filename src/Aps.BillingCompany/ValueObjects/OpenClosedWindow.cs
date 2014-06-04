using System;
using Seterlund.CodeGuard;

namespace Aps.BillingCompanies.ValueObjects
{
    public class OpenClosedWindow
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsOpen { get; private set; }
        public int ConcurrentScrapingLimit { get; private set; }

        protected OpenClosedWindow()
        {
            
        }

        public OpenClosedWindow(DateTime startDate, DateTime endDate, bool isOpen, int concurrentScrapingLimit)
        {
            if (isOpen)
            {
                Guard.That(concurrentScrapingLimit).IsGreaterThan(0);
            }

            Guard.That(startDate).IsTrue(time => time >= DateTime.Now, "startdate cannot be in the past");
            Guard.That(endDate).IsTrue(time => time >= DateTime.Now, "enddate cannot be in the past");

            Guard.That(startDate).IsLessThan(endDate);


            ConcurrentScrapingLimit = concurrentScrapingLimit;
            IsOpen = isOpen;
            EndDate = endDate;
            StartDate = startDate;
        }
    }
}