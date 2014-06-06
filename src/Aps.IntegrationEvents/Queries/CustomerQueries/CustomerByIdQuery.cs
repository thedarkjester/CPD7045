using System;
using Aps.Customers;
using Aps.Customers.Aggregates;
using Aps.Integration.Queries.CustomerQueries.Dtos;

namespace Aps.Integration.Queries.CustomerQueries
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
            Customer customer = customerRepositoryFake.GetCustomerById(id);

            if (customer == null)
            {    
                return null;
            }

            CustomerDto dto = MapCustomerAggregateToCustomerDto(customer);
            
            return dto; 
        }

        public CustomerDto MapCustomerAggregateToCustomerDto(Customer customer)
        {
            CustomerDto dto = new CustomerDto
            {
                FirstName = customer.CustomerFirstName.FirstName,
                LastName = customer.CustomerLastName.LastName,
                Email = customer.CustomerEmailAddress.EmailAddress,
                Telephone = customer.CustomerTelephone.Telephone,
                APSUsername = customer.CustomerAPSUsername.APSUsername,
                APSPassword = customer.CustomerAPSPassword.Password,

            };

            return dto;
        }
    }
}
