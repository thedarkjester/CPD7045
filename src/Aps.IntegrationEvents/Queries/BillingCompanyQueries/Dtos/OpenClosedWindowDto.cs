using System;

namespace Aps.Integration.Queries.BillingCompanyQueries.Dtos
{
    public class OpenClosedWindowDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ConcurrentScrapingLimit { get; set; }
        public bool IsOpen { get; set; }
    }
}