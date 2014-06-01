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
    }
}