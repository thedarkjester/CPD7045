using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aps.Customers.ValueObjects;

namespace Aps.Integration.Queries.CustomerQueries.Dtos
{
    public class CustomerBillingCompanyAccountDto
    {
        public Guid billingCompanyId;
        public string billingCompanyUsername;
        public string billingCompanyPassword;
        public string billingCompanyStatus;
        public int billingCompanyPIN;
        public DateTime dateBillingCompanyAdded;
        public Guid statementId;
        public DateTime statementDate;
    }
}
