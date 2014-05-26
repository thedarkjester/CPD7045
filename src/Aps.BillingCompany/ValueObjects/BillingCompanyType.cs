namespace Aps.BillingCompanies.ValueObjects
{
    public class BillingCompanyType
    {
        public int TypeCode { get; private set; }

        protected BillingCompanyType()
        {

        }

        public BillingCompanyType(int typeCode)
        {
            TypeCode = typeCode;
        }
    }
}