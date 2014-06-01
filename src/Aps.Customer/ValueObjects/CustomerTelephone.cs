using Seterlund.CodeGuard;

namespace Aps.Customers.ValueObjects
{
    public class CustomerTelephone
    {
        private readonly string telephone;

        public string Telephone
        {
            get { return telephone; }
        }

        protected CustomerTelephone()
        {

        }

        public CustomerTelephone(string telephone)
        {
            Guard.That(telephone).IsNotEmpty();

            this.telephone = telephone;
        }

        public CustomerTelephone ChangeTelephone(string newTelephone)
        {
            return new CustomerTelephone(newTelephone);
        }

        public override string ToString()
        {
            return telephone;
        }
    }
}