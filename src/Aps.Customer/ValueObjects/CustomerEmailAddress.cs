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

        public override bool Equals(System.Object obj)
        {
            CustomerEmailAddress email = obj as CustomerEmailAddress;
            if ((object)email == null)
            {
                return false;
            }

            return EmailAddress == email.EmailAddress;
        }

        public bool Equals(CustomerEmailAddress email)
        {
            return EmailAddress == email.EmailAddress;
        }

        public override int GetHashCode()
        {
            return EmailAddress.GetHashCode();
        }

        public static bool operator ==(CustomerEmailAddress a, CustomerEmailAddress b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.EmailAddress == b.EmailAddress;
        }

        public static bool operator !=(CustomerEmailAddress a, CustomerEmailAddress b)
        {
            return !(a == b);
        }

    }
}