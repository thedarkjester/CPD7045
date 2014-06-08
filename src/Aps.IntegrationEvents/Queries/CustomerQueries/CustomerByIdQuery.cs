using System;
using Aps.Customers;
using Aps.Customers.Aggregates;
using Aps.Integration.Queries.CustomerQueries.Dtos;

namespace Aps.Integration.Queries.CustomerQueries
{
    public class CustomerByIdQuery
    {
        private readonly ICustomerRepository ICustomerRepository;
        
        public CustomerByIdQuery(ICustomerRepository ICustomerRepository)
        {
            this.ICustomerRepository = ICustomerRepository;
        }

        public CustomerDto GetCustomerById(Guid id)
        {
            Customer customer = ICustomerRepository.GetCustomerById(id);

            if (customer == null)
            {    
                return null;
            }

            CustomerDto dto = MapCustomerAggregateToCustomerDto(customer);
            
            return dto; 
        }

        private CustomerDto MapCustomerAggregateToCustomerDto(Customer customer)
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
