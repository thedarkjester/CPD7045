using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Customers.ValueObjects
{
    public class CustomerStatement
    {
        public Guid statementId;
        public DateTime statementDate;

        protected CustomerStatement()
        {

        }

        public CustomerStatement(Guid statementId, DateTime statementDate)
        {
            Guard.That(statementId).IsNotEmpty();
            Guard.That(statementDate).IsTrue(date => date >= DateTime.Now, "statementDate cannot be earlier than now");

            this.statementId = statementId;
            this.statementDate = statementDate;
        }

    }
}