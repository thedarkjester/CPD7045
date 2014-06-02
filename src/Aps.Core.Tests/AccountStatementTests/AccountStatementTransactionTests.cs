using System;
using Aps.Core.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.AccountStatementTests
{
    [TestClass]
    public class AccountStatementTransactionTests
    {
        private const decimal TransactionTotal = 11.4M;
        private decimal vatAmount = 1.4M;
        private string transactionDescription = "Ten Rand Transaction";
        private decimal transactionAmount = 10M;

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_ZeroForTransactionAmountWhenConstructingTransaction_ExceptionIsThrown()
        {
            //arrange
            transactionAmount = 0m;
            
            //act
            AccountStatementTransaction accountStatementTransaction = new AccountStatementTransaction(TransactionTotal,vatAmount,transactionDescription,transactionAmount);

            //assert
            //ExpectedException
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Given_EmptyStringForTransactionDescriptionWhenConstructingTransaction_ExceptionIsThrown()
        {
            //arrange
            transactionDescription = "";

            //act
            AccountStatementTransaction accountStatementTransaction = new AccountStatementTransaction(TransactionTotal, vatAmount, transactionDescription, transactionAmount);

            //assert
            //ExpectedException
        }

        [TestMethod]
        public void Given_ValidNonInferrableValuesWhenConstructingTransaction_ValuesRemainUnchanged()
        {
            //arrange
            //testinitialize values

            //act
            AccountStatementTransaction accountStatementTransaction = new AccountStatementTransaction(TransactionTotal, vatAmount, transactionDescription, transactionAmount);

            //assert
            Assert.IsTrue(accountStatementTransaction.TransactionAmount == transactionAmount);
            Assert.IsTrue(accountStatementTransaction.VatAmount == vatAmount);
            Assert.IsTrue(accountStatementTransaction.TransactionDescription == transactionDescription);
            Assert.IsTrue(accountStatementTransaction.TransactionTotal == TransactionTotal);
        }

        [TestMethod]
        public void Given_NoVatAmountValuesWhenConstructingTransaction_ValuesAreCalculated()
        {
            //arrange
            vatAmount = 0;

            //act
            AccountStatementTransaction accountStatementTransaction = new AccountStatementTransaction(TransactionTotal, vatAmount, transactionDescription, transactionAmount);

            //assert
            Assert.IsTrue(accountStatementTransaction.TransactionAmount == transactionAmount);
            Assert.IsTrue(accountStatementTransaction.VatAmount == 1.4M);
            Assert.IsTrue(accountStatementTransaction.TransactionDescription == transactionDescription);
            Assert.IsTrue(accountStatementTransaction.TransactionTotal == TransactionTotal);
        }

        [TestMethod]
        public void Given_TransactionAmountNegativeWhenConstructingTransaction_TheVatIsLeftAtZeroAndTransactionTotalMatchesTheTransactionAmount()
        {
            //arrange
            transactionAmount = -100;

            //act
            AccountStatementTransaction accountStatementTransaction = new AccountStatementTransaction(TransactionTotal, vatAmount, transactionDescription, transactionAmount);

            //assert
            Assert.IsTrue(accountStatementTransaction.TransactionAmount == transactionAmount);
            Assert.IsTrue(accountStatementTransaction.VatAmount == 0M);
            Assert.IsTrue(accountStatementTransaction.TransactionDescription == transactionDescription);
            Assert.IsTrue(accountStatementTransaction.TransactionTotal == transactionAmount);
        }
    }
}
