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

        public override bool Equals(System.Object obj)
        {
            CustomerLastName name = obj as CustomerLastName;
            if ((object)name == null)
            {
                return false;
            }

            return LastName == name.LastName;
        }

        public bool Equals(CustomerLastName name)
        {
            return LastName == name.LastName;
        }

        public override int GetHashCode()
        {
            return LastName.GetHashCode();
        }

        public static bool operator ==(CustomerLastName a, CustomerLastName b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.LastName == b.LastName;
        }

        public static bool operator !=(CustomerLastName a, CustomerLastName b)
        {
            return !(a == b);
        }
    }
}