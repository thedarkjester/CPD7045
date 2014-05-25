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
            ConcurrentScrapes = concurrentScrapes;
        }
    }
}