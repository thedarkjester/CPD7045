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
    }
}