using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Aps.AccountStatements.Entities;
using Aps.AccountStatements.ValueObjects;
using Aps.Core.Constants;
using Aps.Integration;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;
using Aps.Integration.Queries.CustomerQueries;
using Aps.Integration.Queries.CustomerQueries.Dtos;
using Caliburn.Micro;

namespace Aps.Core.Services
{
    public class AccountStatementComposer
    {
        private readonly IEventAggregator eventAggregator;
        private readonly EventIntegrationService eventIntegrationService;
        private readonly CustomerByIdQuery customerByIdQuery;
        private readonly BillingCompanyByIdQuery billingCompanyByIdQuery;

        public AccountStatementComposer(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService, CustomerByIdQuery customerByIdQuery, BillingCompanyByIdQuery billingCompanyByIdQuery)
        {
            this.eventAggregator = eventAggregator;
            this.eventIntegrationService = eventIntegrationService;
            this.customerByIdQuery = customerByIdQuery;
            this.billingCompanyByIdQuery = billingCompanyByIdQuery;
        }

        public AccountStatement BuildAccountStatement(Guid customerId, Guid billingCompanyId, List<KeyValuePair<string, object>> fieldValues)
        {
            CustomerDetails customerDetails = GetCustomerDetails(customerId);
            BillingCompanyDetails billingCompanyDetails = GetBillingCompanyDetails(billingCompanyId);
            StatementDate statementDate = GetStatementDate(fieldValues);
            List<AccountStatementTransaction> statementTransactions = BuildStatementTransactionsFromFieldValues(fieldValues);
            List<AccountLineDetails> accountLineDetails = BuildAccountLineDetailsFieldValues(fieldValues);

            var accountStatement = new AccountStatement(customerDetails, billingCompanyDetails, statementDate, statementTransactions, accountLineDetails);

            return accountStatement;
        }

        private List<AccountLineDetails> BuildAccountLineDetailsFieldValues(List<KeyValuePair<string, object>> fieldValues)
        {
            List<AccountLineDetails> accountLineDetails = new List<AccountLineDetails>();

            foreach (var keyValuePair in fieldValues)
            {
                AccountLineDetails accountLineDetail = BuildAccountLineDetail(keyValuePair);
                if (accountLineDetail != null)
                {
                    accountLineDetails.Add(accountLineDetail);
                }
            }

            return accountLineDetails;
        }

        private AccountLineDetails BuildAccountLineDetail(KeyValuePair<string, object> keyValuePair)
        {
            if (KeyIsLineDetail(keyValuePair.Key))
            {
                return new AccountLineDetails(keyValuePair.Key, keyValuePair.Value.ToString());
            }

            return null;
        }

        private bool KeyIsLineDetail(string key)
        {
            if (StatementFields.TransactionFields.ToList().Contains(key))
            {
                return false;
            }

            if (StatementFields.HeaderFields.ToList().Contains(key))
            {
                return false;
            }

            if (StatementFields.HeaderFields.ToList().Contains(key))
            {
                return false;
            }

            return false;
        }

        private List<AccountStatementTransaction> BuildStatementTransactionsFromFieldValues(List<KeyValuePair<string, object>> fieldValues)
        {
            List<AccountStatementTransaction> accountStatementTransactions = new List<AccountStatementTransaction>();

            foreach (var keyValuePair in fieldValues)
            {
                AccountStatementTransaction transaction = BuildAccountStatementTransaction(keyValuePair);
                if (transaction != null)
                {
                    accountStatementTransactions.Add(transaction);
                }
            }

            return accountStatementTransactions;
        }

        private AccountStatementTransaction BuildAccountStatementTransaction(KeyValuePair<string, object> keyValuePair)
        {
            decimal value = 0;

            if (KeyIsTransaction(keyValuePair.Key))
            {
                decimal.Parse((string)keyValuePair.Value, (NumberStyles)value);

                return new AccountStatementTransaction(0M, 0M, keyValuePair.Key, value);
            }

            return null;
        }

        private bool KeyIsTransaction(string key)
        {
            if (StatementFields.TransactionFields.ToList().Contains(key))
            {
                return true;
            }

            return false;
        }

        private StatementDate GetStatementDate(List<KeyValuePair<string, object>> fieldValues)
        {
            object statmentDate = fieldValues.Find(x => x.Key == "Statement month");

            DateTime statmentMonth;

            bool converted = DateTime.TryParse(statmentDate.ToString(),out statmentMonth);

            if (!converted)
            {
                return new StatementDate(DateTime.UtcNow);
            }

            return new StatementDate(statmentMonth);
        }

        private BillingCompanyDetails GetBillingCompanyDetails(Guid billingCompanyId)
        {
            BillingCompanyDto billingCompanyDto = billingCompanyByIdQuery.GetBillingCompanyById(billingCompanyId);

            return new BillingCompanyDetails(billingCompanyId, billingCompanyDto.Name);
        }

        private CustomerDetails GetCustomerDetails(Guid customerId)
        {
            CustomerDto customerDto = customerByIdQuery.GetCustomerById(customerId);

            return new CustomerDetails(customerId, string.Format("{0} {1}", customerDto.FirstName, customerDto.LastName));
        }
    }
}