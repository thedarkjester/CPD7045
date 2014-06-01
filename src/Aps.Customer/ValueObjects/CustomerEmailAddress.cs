using Seterlund.CodeGuard;

namespace Aps.Customers.ValueObjects
{
    public class CustomerEmailAddress
    {
        private readonly string emailAddress;

        public string EmailAddress
        {
            get { return emailAddress; }
        }

        protected CustomerEmailAddress()
        {

        }

        public CustomerEmailAddress(string emailAddress)
        {
            Guard.That(emailAddress).IsNotEmpty();

            this.emailAddress = emailAddress;
        }

        public CustomerEmailAddress ChangeEmailAddress(string newEmailAddress)
        {
            return new CustomerEmailAddress(newEmailAddress);
        }

        public override string ToString()
        {
            return emailAddress;
        }
    }
}