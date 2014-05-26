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
    }
}