using Seterlund.CodeGuard;

namespace Aps.BillingCompanies.ValueObjects
{
    public class BillingCompanyCrossCheckScrapeEnabled
    {
        private readonly bool crossCheckScrapeEnabled;

        public bool CrossCheckScrapeEnabled
        {
            get { return crossCheckScrapeEnabled; }
        }

        protected BillingCompanyCrossCheckScrapeEnabled()
        {

        }

        public BillingCompanyCrossCheckScrapeEnabled(bool crossCheckScrapeEnabled)
        {
            Guard.That(crossCheckScrapeEnabled).IsValidValue();

            this.crossCheckScrapeEnabled = crossCheckScrapeEnabled;
        }

        public BillingCompanyName ChangeName(string newName)
        {
            return new BillingCompanyName(newName);
        }

        public override bool Equals(System.Object obj)
        {
            BillingCompanyCrossCheckScrapeEnabled crossCheckScrapeEnabledCast = obj as BillingCompanyCrossCheckScrapeEnabled;
            if ((object)crossCheckScrapeEnabledCast == null)
            {
                return false;
            }

            return crossCheckScrapeEnabled == crossCheckScrapeEnabledCast.CrossCheckScrapeEnabled;
        }

        public bool Equals(BillingCompanyCrossCheckScrapeEnabled isCrossCheckScrapeEnabled)
        {
            // Return true if the fields match:
            return CrossCheckScrapeEnabled == isCrossCheckScrapeEnabled.CrossCheckScrapeEnabled;
        }

        public override int GetHashCode()
        {
            return CrossCheckScrapeEnabled.GetHashCode();
        }

        public static bool operator ==(BillingCompanyCrossCheckScrapeEnabled a, BillingCompanyCrossCheckScrapeEnabled b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.CrossCheckScrapeEnabled == b.CrossCheckScrapeEnabled;
        }

        public static bool operator !=(BillingCompanyCrossCheckScrapeEnabled a, BillingCompanyCrossCheckScrapeEnabled b)
        {
            return !(a == b);
        }
    }
}