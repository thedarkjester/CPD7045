using Seterlund.CodeGuard;

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
            Guard.That(daysPerBillingCycle).IsGreaterThan(0);

            RetryInterval = retryInterval;
            LeadTimeInterval = leadTimeInterval;
            DaysPerBillingCycle = daysPerBillingCycle;
        }
    }
}