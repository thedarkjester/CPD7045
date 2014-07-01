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

        public override bool Equals(System.Object obj)
        {
            CustomerTelephone tel = obj as CustomerTelephone;
            if ((object)tel == null)
            {
                return false;
            }

            return Telephone == tel.Telephone;
        }

        public bool Equals(CustomerTelephone tel)
        {
            return Telephone == tel.Telephone;
        }

        public override int GetHashCode()
        {
            return Telephone.GetHashCode();
        }

        public static bool operator ==(CustomerTelephone a, CustomerTelephone b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Telephone == b.Telephone;
        }

        public static bool operator !=(CustomerTelephone a, CustomerTelephone b)
        {
            return !(a == b);
        }
    }
}