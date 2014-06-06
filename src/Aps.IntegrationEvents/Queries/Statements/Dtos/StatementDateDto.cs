using System;

namespace Aps.Integration.Queries.Statements.Dtos
{
    public class StatementDateDto
    {
        public DateTime DateOfStatement { get; set; }

        public StatementDateDto(DateTime dateTime)
        {
            DateOfStatement = dateTime;
        }
    }
}