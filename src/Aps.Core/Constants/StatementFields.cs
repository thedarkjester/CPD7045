using System.Collections.Generic;

namespace Aps.Core.Constants
{
    //we could store this in the DB, but this is a prototype
    public static class StatementFields
    {
        private static List<string> transactionFields = new List<string>();
        private static List<string> consumptionFields = new List<string>();
        private static List<string> notificationFields = new List<string>();
        private static List<string> headerFields = new List<string>();
        private static List<string> nonTransactionalFinancialFields = new List<string>();

        public static IEnumerable<string> TransactionFields
        {
            get { return transactionFields; }
        }

        public static IEnumerable<string> ConsumptionFields
        {
            get { return consumptionFields; }
        }

        public static IEnumerable<string> NotificationFields
        {
            get { return notificationFields; }
        }

        public static IEnumerable<string> HeaderFields
        {
            get { return headerFields; }
        }

        public static IEnumerable<string> NonTransactionalFinancialFields
        {
            get { return nonTransactionalFinancialFields; }
        }

        public static string VatField
        {
            get { return "VAT Amount"; }
        }

        static StatementFields()
        {
            AddTransactionFields();
            AddConsumptionFields();
            AddNoticiationFields();
            AddHeaderFields();
            AddNonTransactionalFinancialFields();
        }

        private static void AddHeaderFields()
        {
            headerFields.Add("Account number");
            headerFields.Add("Account holder name");
            headerFields.Add("Statement date");
            headerFields.Add("Statement number");
            headerFields.Add("Statement month");

        }

        private static void AddNonTransactionalFinancialFields()
        {
            nonTransactionalFinancialFields.Add("Total due");
            nonTransactionalFinancialFields.Add("Due date");
            nonTransactionalFinancialFields.Add("New charges");
            nonTransactionalFinancialFields.Add("Deductions");
            nonTransactionalFinancialFields.Add("Discount");
            nonTransactionalFinancialFields.Add("VAT amount");
        }

        private static void AddNoticiationFields()
        {
            notificationFields.Add("Card type");
            notificationFields.Add("Interest rate");
            notificationFields.Add("Credit limit");
            notificationFields.Add("Credit available");
            notificationFields.Add("Minimum amount due");
            notificationFields.Add("Telephone number");
        }

        private static void AddConsumptionFields()
        {
            consumptionFields.Add("Instalment notice");
            consumptionFields.Add("Electricity used");
            consumptionFields.Add("Gas used");
            consumptionFields.Add("Water used");
            consumptionFields.Add("Total number of calls");
            consumptionFields.Add("Total call duration");
        }

        private static void AddTransactionFields()
        {
            transactionFields.Add("Electricity charges");
            transactionFields.Add("Gas charges");
            transactionFields.Add("Water charges");
            transactionFields.Add("Sewerage charges");
            transactionFields.Add("Refuse charges");
            transactionFields.Add("Service charges");
            transactionFields.Add("Call charges");
            transactionFields.Add("Discount");
            transactionFields.Add("Payment received");
            transactionFields.Add("Opening balance");
        }
    }
}