using System;
using Aps.ApsCustomer;

namespace Aps.Integration.Queries.Customer
{
    public class CustomerByIdQuery
    {
        private readonly CustomerRepositoryFake customerRepositoryFake;

        public CustomerByIdQuery(CustomerRepositoryFake customerRepositoryFake)
        {
            this.customerRepositoryFake = customerRepositoryFake;
        }

        public CustomerDto GetCustomerById(Guid id)
        {
            return new CustomerDto();
        }
    }
}
