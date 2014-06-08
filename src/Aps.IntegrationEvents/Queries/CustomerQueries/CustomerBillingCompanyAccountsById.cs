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

            CustomerBillingCompanyAccount account = customer.CustomerBillingCompanyAccounts.FirstOrDefault(customerBillingCompanyAccount => customerBillingCompanyAccount.billingCompanyId == billingCompanyId);
            
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
                billingCompanyId = customerBillingCompanyAccount.billingCompanyId,
                billingCompanyUsername = customerBillingCompanyAccount.billingCompanyUsername,
                billingCompanyPassword = customerBillingCompanyAccount.billingCompanyPassword,
                billingCompanyStatus = customerBillingCompanyAccount.billingCompanyStatus,
                billingCompanyAccountNumber = customerBillingCompanyAccount.billingCompanyAccountNumber,
                billingCompanyPIN = customerBillingCompanyAccount.billingCompanyPIN,
                dateBillingCompanyAdded = customerBillingCompanyAccount.dateBillingCompanyAdded,
                statementId = customerBillingCompanyAccount.customerStatement.statementId,
                statementDate = customerBillingCompanyAccount.customerStatement.statementDate
            };

            return dto;
        }
    }
}
