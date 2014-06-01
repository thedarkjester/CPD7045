using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aps.BillingCompanies;
using Aps.Core.InternalEvents;
using Aps.Customers;
using Aps.IntegrationEvents;
using Caliburn.Micro;
using Seterlund.CodeGuard;

namespace Aps.Core
{
    public class FailureHandler
    {
        
        public FailureHandler()
        {
        }

        public void ProcessNewFailure(Guid CustomerId, Guid BillingCompanyId, int ErrorNum)
        {
            Guard.That(CustomerId).IsNotEmpty();
            Guard.That(BillingCompanyId).IsNotEmpty();
            //Guard.That(ErrorNum).IsNot??

            string emailBody;
            string emailSubject;
            string customerEmail;

            switch (ErrorNum)
            {

                // Invalid Credentials Error
                case 0:

                    // Change Customer Status to Inactive
                    // Find out how to send an event to the customer to change status
                    // Notify customer via email
                    emailSubject = "Invalid Credentials on ??? "; // how do I get a billing company name here?
                    emailBody = "Hi,\n Your credentials were invalid on the {0} website.\n Please Action. \n Regards, \nAPS Team"; // add company to {0}
                    break;

                // Invalid Credentials Error
                case 1:

                    // Change Customer Status to Inactive
                    // Find out how to send an event to the customer to change status
                    // Notify customer via email
                    emailSubject = "Invalid Credentials on ??? "; // how do I get a billing company name here?
                    emailBody = "Hi,\n Your credentials were invalid on the {0} website.\n Please Action. \n Regards, \nAPS Team"; // add company to {0}
                    break;

                // Not signed up for e-Billing
                case 2:

                    // Change Customer Status to Inactive
                    // Find out how to send an event to the customer to change status
                    // Notify customer via email
                    emailSubject = "Invalid Credentials on ??? "; // how do I get a billing company name here?
                    emailBody = "Hi,\n Your credentials were invalid on the {0} website.\n Please Action. \n Regards, \nAPS Team"; // add company to {0}
                    break;

                // Billing Company action required
                case 3:

                    // Change Customer Status to Inactive
                    // Find out how to send an event to the customer to change status
                    // Notify customer via email
                    emailSubject = "Invalid Credentials on ??? "; // how do I get a billing company name here?
                    emailBody = "Hi,\n Your credentials were invalid on the {0} website.\n Please Action. \n Regards, \nAPS Team"; // add company to {0}
                    break;

                // Billing Company site down - Unscheduled maintenance
                case 4:

                    // Change Customer Status to Trying
                    // Find out how to send an event to the customer to change status
                    // Delay Scrape for a possibly determined timeframe
                    // How do I delay the scraper? send an event?
                    break;

                // Error Page Encountered
                case 5:

                    // Change Customer Status to Trying
                    // Find out how to send an event to the customer to change status
                    // Delay Scrape for a short Time frame
                    // How do I delay the scraper? send an event?
                    break;

                // Broken Script - Site Changed
                case 6:

                    // Change Customer Status to Trying 
                    // Find out how to send an event to the customer to change status
                    // Notify Production Support
                    customerEmail = "Support@APS.co.za";
                    emailSubject = "Broken Script - Site Changed"; // how do I get a billing company name here?
                    emailBody = "HiBroken Script - Site Changed";
                    break;

                // Broken Script - Unhandled Data Condition
                case 7:
                    
                    // Change Customer Status to Trying
                    // Find out how to send an event to the customer to change status
                    // Notify Production Support
                    customerEmail = "Support@APS.co.za";
                    emailSubject = "Broken Script - Unhandled Data Condition"; // how do I get a billing company name here?
                    emailBody = "Broken Script - Unhandled Data Condition";
                    break;
            }

        }

        public void SendMail(String CustomerEmail, String emailSubject, String emailBody)
        {
            EmailMessage mail = new EmailMessage();

            mail.SendEmail(CustomerEmail, emailSubject, emailBody);

        }

    }
}
