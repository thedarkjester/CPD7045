using Seterlund.CodeGuard;

namespace Aps.Customers.ValueObjects
{
    public class CustomerFirstName
    {
        private readonly string firstName;

        public string FirstName
        {
            get { return firstName; }
        }

        protected CustomerFirstName()
        {

        }

        public CustomerFirstName(string firstName)
        {
            Guard.That(firstName).IsNotEmpty();

            this.firstName = firstName;
        }

        public CustomerFirstName ChangeFirstName(string newFirstName)
        {
            return new CustomerFirstName(newFirstName);
        }

        public override string ToString()
        {
            return firstName;
        }

        public override bool Equals(System.Object obj)
        {
            CustomerFirstName name = obj as CustomerFirstName;
            if ((object)name == null)
            {
                return false;
            }

            return FirstName == name.FirstName;
        }

        public bool Equals(CustomerFirstName name)
        {
            return FirstName == name.FirstName;
        }

        public override int GetHashCode()
        {
            return FirstName.GetHashCode();
        }

        public static bool operator ==(CustomerFirstName a, CustomerFirstName b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.FirstName == b.FirstName;
        }

        public static bool operator !=(CustomerFirstName a, CustomerFirstName b)
        {
            return !(a == b);
        }
    }
}