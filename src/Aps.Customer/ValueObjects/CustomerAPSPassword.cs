using Seterlund.CodeGuard;

namespace Aps.Customers.ValueObjects
{
    public class CustomerAPSPassword
    {
        private readonly string password;

        public string Password
        {
            get { return password; }
        }

        protected CustomerAPSPassword()
        {

        }

        public CustomerAPSPassword(string password)
        {
            Guard.That(password).IsNotEmpty();

            this.password = password;
        }

        public CustomerAPSPassword ChangeAPSPassword(string newPassword)
        {
            return new CustomerAPSPassword(newPassword);
        }

        public override string ToString()
        {
            return password;
        }

        public override bool Equals(System.Object obj)
        {
            CustomerAPSPassword password = obj as CustomerAPSPassword;
            if ((object)password == null)
            {
                return false;
            }

            return Password == password.Password;
        }

        public bool Equals(CustomerAPSPassword password)
        {
            return Password == password.Password;
        }

        public override int GetHashCode()
        {
            return Password.GetHashCode();
        }

        public static bool operator ==(CustomerAPSPassword a, CustomerAPSPassword b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Password == b.Password;
        }

        public static bool operator !=(CustomerAPSPassword a, CustomerAPSPassword b)
        {
            return !(a == b);
        }
    }
}