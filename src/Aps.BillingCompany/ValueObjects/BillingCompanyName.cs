using Seterlund.CodeGuard;

namespace Aps.BillingCompanies.ValueObjects
{
    public class BillingCompanyName
    {
        private readonly string name;

        public string Name
        {
            get { return name; }
        }

        protected BillingCompanyName()
        {

        }

        public BillingCompanyName(string name)
        {
            Guard.That(name).IsNotEmpty();

            this.name = name;
        }

        public BillingCompanyName ChangeName(string newName)
        {
            return new BillingCompanyName(newName);
        }

        public override string ToString()
        {
            return name;
        }

        public override bool Equals(System.Object obj)
        {
            BillingCompanyName companyName = obj as BillingCompanyName;
            if ((object)companyName == null)
            {
                return false;
            }

            return  Name == companyName.Name;
        }

        public bool Equals(BillingCompanyName companyName)
        {
            // Return true if the fields match:
            return Name == companyName.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(BillingCompanyName a, BillingCompanyName b)
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
            return a.Name == b.Name;
        }

        public static bool operator !=(BillingCompanyName a, BillingCompanyName b)
        {
            return !(a == b);
        }
    }
}