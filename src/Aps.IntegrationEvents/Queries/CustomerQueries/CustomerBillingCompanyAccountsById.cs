using System;
using System.Linq;
using Aps.Customers;
using Aps.Customers.Aggregates;
using Aps.Customers.Entities;
using Aps.Integration.Queries.CustomerQueries.Dtos;

namespace Aps.Integration.Queries.CustomerQueries.Dtos
{
    public class CustomerBillingCompanyAccountsById
    {
        private readonly ICustomerRepository ICustomerRepository;

        public CustomerBillingCompanyAccountsById(ICustomerRepository ICustomerRepository)
        {
            this.ICustomerRepository = ICustomerRepository;
        }

        public CustomerBillingCompanyAccountDto GetCustomerBillingCompanyAccountByCustomerIdAndBillingCompanyId(Guid customerId, Guid billingCompanyId)
        {
            Customer customer = ICustomerRepository.GetCustomerById(customerId);

            if (customer == null)
            {    
                return null;
            }

            CustomerBillingCompanyAccount account = customer.CustomerBillingCompanyAccounts.FirstOrDefault(customerBillingCompanyAccount => customerBillingCompanyAccount.BillingCompanyId == billingCompanyId);
            
            if(account == null)
            {
                return null;
            }

            CustomerBillingCompanyAccountDto dto = MapCustomerAggregateToCustomerBillingCompanyAccountDto(account);
            
            return dto; 
        }

        private CustomerBillingCompanyAccountDto MapCustomerAggregateToCustomerBillingCompanyAccountDto(CustomerBillingCompanyAccount customerBillingCompanyAccount)
        {
            CustomerBillingCompanyAccountDto dto = new CustomerBillingCompanyAccountDto
            {
                billingCompanyId = customerBillingCompanyAccount.BillingCompanyId,
                billingCompanyUsername = customerBillingCompanyAccount.BillingCompanyUsername,
                billingCompanyPassword = customerBillingCompanyAccount.BillingCompanyPassword,
                billingCompanyStatus = customerBillingCompanyAccount.BillingCompanyStatus,
                billingCompanyPIN = customerBillingCompanyAccount.BillingCompanyPIN,
                dateBillingCompanyAdded = customerBillingCompanyAccount.DateBillingCompanyAdded,
                statementId = customerBillingCompanyAccount.CustomerStatement.statementId,
                statementDate = customerBillingCompanyAccount.CustomerStatement.statementDate,
                billingCompanyAccountNumber = customerBillingCompanyAccount.BillingCompanyAccountNumber
            };

            return dto;
        }
    }
}
