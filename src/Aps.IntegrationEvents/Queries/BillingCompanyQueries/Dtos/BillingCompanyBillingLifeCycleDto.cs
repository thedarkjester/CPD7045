using System;

namespace Aps.Integration.Queries.BillingCompanyQueries.Dtos
{
    public class BillingCompanyBillingLifeCycleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DaysPerBillingCycle { get; set; }
        public int LeadTimeInterval { get; set; }
        public int RetryInterval { get; set; }
    }
}