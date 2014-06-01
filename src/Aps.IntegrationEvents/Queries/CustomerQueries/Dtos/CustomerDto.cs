using System;

namespace Aps.Integration.Queries.CustomerQueries.Dtos
{
    public class CustomerDto    
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Telephone { get; set; }
        public String APSUsername{ get; set; }
        public String APSPassword{ get; set; }
    }
}