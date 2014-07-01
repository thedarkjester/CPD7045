using Seterlund.CodeGuard;

namespace Aps.Customers.ValueObjects
{
    public class CustomerAPSUsername
    {
        private readonly string username;

        public string APSUsername
        {
            get { return username; }
        }

        protected CustomerAPSUsername()
        {

        }

        public CustomerAPSUsername(string username)
        {
            Guard.That(username).IsNotEmpty();

            this.username = username;
        }

        public CustomerAPSUsername ChangeUsername(string newUsername)
        {
            return new CustomerAPSUsername(newUsername);
        }

        public override string ToString()
        {
            return username;
        }

        public override bool Equals(System.Object obj)
        {
            CustomerAPSUsername username = obj as CustomerAPSUsername;
            if ((object)username == null)
            {
                return false;
            }

            return APSUsername == username.APSUsername;
        }

        public bool Equals(CustomerAPSUsername username)
        {
            return APSUsername == username.APSUsername;
        }

        public override int GetHashCode()
        {
            return APSUsername.GetHashCode();
        }

        public static bool operator ==(CustomerAPSUsername a, CustomerAPSUsername b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.APSUsername == b.APSUsername;
        }

        public static bool operator !=(CustomerAPSUsername a, CustomerAPSUsername b)
        {
            return !(a == b);
        }
    }
}