using System;
using Seterlund.CodeGuard;

namespace Aps.BillingCompanies.ValueObjects
{
    public class OpenClosedScrapingWindow
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsOpen { get; private set; }
        public int ConcurrentScrapingLimit { get; private set; }

        protected OpenClosedScrapingWindow()
        {
            
        }

        public OpenClosedScrapingWindow(DateTime startDate, DateTime endDate, bool isOpen, int concurrentScrapingLimit)
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

        public override bool Equals(System.Object obj)
        {
            OpenClosedScrapingWindow openClosedScrapingWindow = obj as OpenClosedScrapingWindow;
            if ((object)openClosedScrapingWindow == null)
            {
                return false;
            }

            return StartDate == openClosedScrapingWindow.StartDate 
                && EndDate == openClosedScrapingWindow.EndDate 
                && IsOpen == openClosedScrapingWindow.IsOpen 
                && ConcurrentScrapingLimit == openClosedScrapingWindow.ConcurrentScrapingLimit;
        }

        public bool Equals(OpenClosedScrapingWindow openClosedScrapingWindow)
        {
            return StartDate == openClosedScrapingWindow.StartDate 
                && EndDate == openClosedScrapingWindow.EndDate 
                && IsOpen == openClosedScrapingWindow.IsOpen 
                && ConcurrentScrapingLimit == openClosedScrapingWindow.ConcurrentScrapingLimit;
        }

        public override int GetHashCode()
        {
            return StartDate.GetHashCode() ^ EndDate.GetHashCode() 
                ^ IsOpen.GetHashCode() ^ ConcurrentScrapingLimit.GetHashCode();
        }

        public static bool operator ==(OpenClosedScrapingWindow a, OpenClosedScrapingWindow b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.StartDate == b.StartDate && a.EndDate == b.EndDate 
                && a.IsOpen == b.IsOpen && a.ConcurrentScrapingLimit == b.ConcurrentScrapingLimit;
        }

        public static bool operator !=(OpenClosedScrapingWindow a, OpenClosedScrapingWindow b)
        {
            return !(a == b);
        }
    }
}