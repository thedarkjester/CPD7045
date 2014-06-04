using System;
using System.Collections.Generic;
using Aps.AccountStatements;
using Aps.AccountStatements.Entities;
using Aps.AccountStatements.ValueObjects;
using Aps.Integration.Queries.Statements.Dtos;

namespace Aps.Integration.Queries.Statements
{
    public class GetAccountStatementByIdQuery
    {
        private readonly AccountStatementRepositoryFake accountStatementRepository;

        public GetAccountStatementByIdQuery(AccountStatementRepositoryFake accountStatementRepository)
        {
            this.accountStatementRepository = accountStatementRepository;
        }

        public AccountStatementDto GetAccountStatement(Guid id)
        {
            AccountStatement accountStatement = accountStatementRepository.GetAccountStatementById(id);

            if (accountStatement == null)
            {
                return null;
            }

            AccountStatementDto dto = MapAccountStatementToDto(accountStatement);

            return dto;
        }

        private AccountStatementDto MapAccountStatementToDto(AccountStatement accountStatement)
        {
            CustomerDetailsDto customerDetailsDto = MapCustomerDetailsDto(accountStatement);
            BillingCompanyDetailsDto billingCompanyDetailsDto = MapBillingCompanyDetailsDto(accountStatement);
            StatementDateDto statementDateDto = MapStatementDateDto(accountStatement);
            var accountStatementTransactionDtos = MapAccountStatementTransactionDtos(accountStatement);

            AccountStatementDto accountStatementDto = new AccountStatementDto(customerDetailsDto, billingCompanyDetailsDto, statementDateDto, accountStatementTransactionDtos);

            return accountStatementDto;
        }

        private List<AccountStatementTransactionDto> MapAccountStatementTransactionDtos(AccountStatement accountStatement)
        {
            List<AccountStatementTransactionDto> accountStatementTransactionDtos = new List<AccountStatementTransactionDto>();

            foreach (AccountStatementTransaction accountStatementTransaction in accountStatement.StatementTransactions)
            {
                accountStatementTransactionDtos.Add(MapAccountStatementTransactionToDto(accountStatementTransaction));
            }

            return accountStatementTransactionDtos;
        }

        private static StatementDateDto MapStatementDateDto(AccountStatement accountStatement)
        {
            StatementDateDto statementDateDto = new StatementDateDto(accountStatement.StatementDate.DateOfStatement);
            return statementDateDto;
        }

        private static BillingCompanyDetailsDto MapBillingCompanyDetailsDto(AccountStatement accountStatement)
        {
            BillingCompanyDetailsDto billingCompanyDetailsDto =
                new BillingCompanyDetailsDto(accountStatement.BillingCompanyDetails.BillingCompanyId,
                                             accountStatement.BillingCompanyDetails.CompanyName);
            return billingCompanyDetailsDto;
        }

        private static CustomerDetailsDto MapCustomerDetailsDto(AccountStatement accountStatement)
        {
            CustomerDetailsDto customerDetailsDto = new CustomerDetailsDto(accountStatement.CustomerDetails.CustomerId,
                                                                           accountStatement.CustomerDetails.CustomerName);
            return customerDetailsDto;
        }

        private AccountStatementTransactionDto MapAccountStatementTransactionToDto(AccountStatementTransaction accountStatementTransaction)
        {
            AccountStatementTransactionDto accountStatementTransactionDto = new AccountStatementTransactionDto(accountStatementTransaction.TransactionTotal, accountStatementTransaction.VatAmount, accountStatementTransaction.TransactionDescription, accountStatementTransaction.TransactionAmount);

            return accountStatementTransactionDto;
        }
    }
}
