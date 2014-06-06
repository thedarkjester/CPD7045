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
    }
}