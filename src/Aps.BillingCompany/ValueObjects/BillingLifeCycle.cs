namespace Aps.BillingCompanies.ValueObjects
{
    public class BillingLifeCycle
    {
        public int DaysPerBillingCycle { get; private set; }
        public int LeadTimeInterval { get; private set; }
        public int RetryInterval { get; private set; }

        protected BillingLifeCycle()
        {
            
        }

        public BillingLifeCycle(int daysPerBillingCycle, int leadTimeInterval, int retryInterval)
        {
            RetryInterval = retryInterval;
            LeadTimeInterval = leadTimeInterval;
            DaysPerBillingCycle = daysPerBillingCycle;
        }
    }
}