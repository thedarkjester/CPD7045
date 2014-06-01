namespace Aps.Integration.Queries.BillingCompanyQueries.Dtos
{
    public class ScrapingErrorRetryConfigurationDto
    {
        public int ResponseCode { get; set; }
        public int NumberOfRetries { get; set; }
    }
}