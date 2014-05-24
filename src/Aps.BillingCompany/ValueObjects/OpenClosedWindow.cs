using System;

namespace Aps.BillingCompanies.ValueObjects
{
    public class OpenClosedWindow
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsOpen { get; private set; }
        public int ConcurrentScrapingLimit { get; private set; }

        private OpenClosedWindow()
        {
            
        }

        public OpenClosedWindow(DateTime startDate, DateTime endDate, bool isOpen, int concurrentScrapingLimit)
        {
            ConcurrentScrapingLimit = concurrentScrapingLimit;
            IsOpen = isOpen;
            EndDate = endDate;
            StartDate = startDate;
        }
    }
}