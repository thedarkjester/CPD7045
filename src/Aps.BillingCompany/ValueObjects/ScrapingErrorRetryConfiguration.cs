namespace Aps.BillingCompanies.ValueObjects
{
    public class ScrapingErrorRetryConfiguration
    {
        public int ResponseCode { get; private set; }
        public int NumberOfRetries { get; private set; }

        protected ScrapingErrorRetryConfiguration()
        {

        }

        public ScrapingErrorRetryConfiguration(int responseCode, int numberOfRetries)
        {
            NumberOfRetries = numberOfRetries;
            ResponseCode = responseCode;
        }
    }
}