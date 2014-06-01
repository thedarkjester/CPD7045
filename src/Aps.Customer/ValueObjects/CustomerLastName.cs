using Seterlund.CodeGuard;

namespace Aps.Customers.ValueObjects
{
    public class CustomerLastName
    {
        private readonly string lastName;

        public string LastName
        {
            get { return lastName; }
        }

        protected CustomerLastName()
        {

        }

        public CustomerLastName(string lastName)
        {
            Guard.That(lastName).IsNotEmpty();

            this.lastName = lastName;
        }

        public CustomerLastName ChangeLastName(string newLastName)
        {
            return new CustomerLastName(newLastName);
        }

        public override string ToString()
        {
            return lastName;
        }
    }
}