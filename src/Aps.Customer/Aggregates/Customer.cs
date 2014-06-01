using Aps.Customers.ValueObjects;
using Aps.DomainBase;
using Caliburn.Micro;

namespace Aps.Customers.Aggregates
{
    public class Customer : Aggregate
    {
        private readonly IEventAggregator eventAggregator;

        private CustomerFirstName customerFirstName;
        private CustomerLastName customerLastName;
        private CustomerEmailAddress customerEmailAddress;
        private CustomerTelephone customerTelephone;
        private CustomerAPSUsername customerAPSUsername;
        private CustomerAPSPassword customerAPSPassword;

        private Customer()
        {

        }
                
        public Customer(IEventAggregator aggregator, CustomerFirstName customerFirstName, CustomerLastName customerLastName, CustomerEmailAddress customerEmailAddress,
                        CustomerTelephone customerTelephone, CustomerAPSUsername customerAPSUsername, CustomerAPSPassword customerAPSPassword)
        {
            this.customerFirstName = customerFirstName;
            this.customerLastName = customerLastName;
            this.customerEmailAddress = customerEmailAddress;
            this.customerTelephone = customerTelephone;
            this.customerAPSUsername = customerAPSUsername;
            this.customerAPSPassword = customerAPSPassword;

            this.eventAggregator = aggregator;
            this.eventAggregator.Subscribe(this);

        }

        public CustomerFirstName CustomerFirstName
        {
            get { return this.customerFirstName; }
        }

        public CustomerLastName CustomerLastName
        {
            get { return this.customerLastName; }
        }
        
        public CustomerEmailAddress CustomerEmailAddress
        {
            get { return this.CustomerEmailAddress; }
        }
        
        public CustomerTelephone CustomerTelephone
        {
            get { return this.customerTelephone; }
        }
        
        public CustomerAPSUsername CustomerAPSUsername
        {
            get { return this.customerAPSUsername; }
        }
        
        public CustomerAPSPassword CustomerAPSPassword
        {
            get { return this.customerAPSPassword; }
        }

        public void SetCustomerFirstName(CustomerFirstName firstName)
        {
            // validation of action

            this.customerFirstName = firstName;
        }

        public void SetCustomerLastName(CustomerLastName lastName)
        {
            // validation of action

            this.customerLastName = lastName;
        }

        public void SetCustomerEmailAddress(CustomerEmailAddress email)
        {
            // validation of action

            this.customerEmailAddress = email;
        }

        public void SetCustomerTelephone(CustomerTelephone telephone)
        {
            // validation of action

            this.customerTelephone = telephone;
        }

        public void SetCustomerAPSUsername(CustomerAPSUsername username)
        {
            // validation of action

            this.customerAPSUsername = username;
        }

        public void SetCustomerPassword(CustomerAPSPassword password)
        {
            // validation of action

            this.customerAPSPassword = password;
        }

    }
}
