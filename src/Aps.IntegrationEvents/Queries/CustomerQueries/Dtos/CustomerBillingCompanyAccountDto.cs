using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Integration.Queries.CustomerQueries.Dtos
{
    class CustomerBillingCompanyAccountDto
    {
        public Guid billingCompanyId;
        public string billingCompanyUsername;
        public string billingCompanyPassword;
        public string billingCompanyStatus;
        public int billingCompanyPIN;
        public DateTime dateBillingCompanyAdded;
    }
}
