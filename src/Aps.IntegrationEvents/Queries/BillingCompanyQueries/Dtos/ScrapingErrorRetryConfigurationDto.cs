using Aps.Integration.EnumTypes;

namespace Aps.Integration.Queries.BillingCompanyQueries.Dtos
{
    public class ScrapingErrorRetryConfigurationDto
    {
        public ScrapingErrorResponseCodes ResponseCode { get; set; }
        public int RetryInterval { get; set; }
    }
}