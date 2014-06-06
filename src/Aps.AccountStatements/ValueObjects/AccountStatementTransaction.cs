using Seterlund.CodeGuard;

namespace Aps.AccountStatements.ValueObjects
{
    public class AccountStatementTransaction
    {
        public decimal TransactionAmount { get; private set; }
        public string TransactionDescription { get; private set; }
        public decimal VatAmount { get; private set; }
        public decimal TransactionTotal { get; private set; }

        public AccountStatementTransaction(decimal transactionTotal, decimal vatAmount, string transactionDescription, decimal transactionAmount)
        {
            Guard.That(transactionAmount).IsNotEqual(0);
            Guard.That(transactionDescription).IsNotEmpty();

            TransactionAmount = transactionAmount;
            TransactionDescription = transactionDescription;
            VatAmount = vatAmount;
            TransactionTotal = transactionTotal;

            InferValuesFromInputs();
        }

        private void InferValuesFromInputs()
        {
            if (VatAmount == 0M)
            {
                VatAmount = TransactionAmount * 0.14M;
            }

            if (TransactionTotal == 0M)
            {
                TransactionTotal = TransactionAmount + VatAmount;
            }

            if (TransactionAmount < 0M)
            {
                VatAmount = 0M;
                TransactionTotal = TransactionAmount;
            }
        }
    }
}