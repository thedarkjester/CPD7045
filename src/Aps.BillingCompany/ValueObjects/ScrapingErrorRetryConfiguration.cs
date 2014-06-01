using Seterlund.CodeGuard;

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
            Guard.That(responseCode).IsGreaterThan(-1);
            Guard.That(numberOfRetries).IsGreaterThan(0);

            NumberOfRetries = numberOfRetries;
            ResponseCode = responseCode;
        }
    }
}