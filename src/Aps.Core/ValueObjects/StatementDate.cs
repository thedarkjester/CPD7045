using System;

namespace Aps.Core.ValueObjects
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