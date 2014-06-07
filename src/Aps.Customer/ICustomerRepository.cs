using System;
using Aps.Customers.Aggregates;
using Aps.Customers.ValueObjects;

namespace Aps.Customers
{
    public interface ICustomerRepository
    {
        void StoreCustomer(Customer customer);

        Customer GetNewCustomer(CustomerFirstName customerFirstName, CustomerLastName customerLastName, CustomerEmailAddress customerEmailAddress, 
                                                CustomerTelephone customerTelephone, CustomerAPSUsername customerAPSUsername, CustomerAPSPassword customerAPSPassword);

        Customer GetCustomerById(Guid id);
    }
}