using System;

namespace Aps.Core.Models
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