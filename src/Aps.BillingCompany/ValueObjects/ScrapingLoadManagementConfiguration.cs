using Seterlund.CodeGuard;

namespace Aps.BillingCompanies.ValueObjects
{
    public class ScrapingLoadManagementConfiguration
    {
        public int ConcurrentScrapes { get; private set; }

        protected ScrapingLoadManagementConfiguration()
        {

        }

        public ScrapingLoadManagementConfiguration(int concurrentScrapes)
        {
            Guard.That(concurrentScrapes).IsGreaterThan(0);

            ConcurrentScrapes = concurrentScrapes;
        }
    }
}