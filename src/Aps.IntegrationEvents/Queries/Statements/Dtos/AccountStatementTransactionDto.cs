using Seterlund.CodeGuard;

namespace Aps.Integration.Queries.Statements.Dtos
{
    public class AccountStatementTransactionDto
    {
        public decimal TransactionAmount { get; private set; }
        public string TransactionDescription { get; private set; }
        public decimal VatAmount { get; private set; }
        public decimal TransactionTotal { get; private set; }

        public AccountStatementTransactionDto(decimal transactionTotal, decimal vatAmount, string transactionDescription, decimal transactionAmount)
        {
            Guard.That(transactionAmount).IsNotEqual(0);
            Guard.That(transactionDescription).IsNotEmpty();

            TransactionAmount = transactionAmount;
            TransactionDescription = transactionDescription;
            VatAmount = vatAmount;
            TransactionTotal = transactionTotal;
        }
    }
}