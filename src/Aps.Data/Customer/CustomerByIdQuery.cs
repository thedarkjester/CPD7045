using System;
using Aps.Customer;

namespace Aps.IntegratedQueries.Customer
{
    public class CustomerByIdQuery
    {
        private readonly CustomerRepository customerRepository;

        public CustomerByIdQuery(CustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public CustomerDto GetCustomerById(Guid id)
        {
            return new CustomerDto();
        }
    }
}
