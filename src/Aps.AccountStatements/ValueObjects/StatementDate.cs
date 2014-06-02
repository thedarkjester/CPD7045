using System;

namespace Aps.AccountStatements.ValueObjects
{
    public class StatementDate
    {
        public DateTime DateOfStatement { get; set; }

        public StatementDate(DateTime dateTime)
        {
            DateOfStatement = dateTime;
        }
    }
}