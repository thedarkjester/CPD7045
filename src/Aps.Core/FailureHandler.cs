using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aps.BillingCompanies;
using Aps.Core.InternalEvents;
using Aps.Customers;
using Aps.Integration;
using Caliburn.Micro;
using Seterlund.CodeGuard;
using Aps.Integration.EnumTypes;
using Aps.Integration.Queries.CustomerQueries;
using Aps.Integration.Queries.CustomerQueries.Dtos;
using Aps.Integration;
using Aps.Integration.Events;


namespace Aps.Core
{
    public class FailureHandler
    {
        private readonly EventIntegrationService eventIntegrationService;
        private readonly IEventAggregator eventAggregator;
        private readonly CustomerByIdQuery customerByIdQuery;

        public FailureHandler(IEventAggregator eventAggregator, CustomerByIdQuery customerByIdQuery, EventIntegrationService eventIntegrationService)
        {
            this.eventAggregator = eventAggregator;
            this.customerByIdQuery = customerByIdQuery;
            this.eventIntegrationService = eventIntegrationService;
        }

        public void ProcessNewFailure(Guid customerId, Guid billingCompanyId, ScrapingErrorResponseCodes errorNum)
        {
            Guard.That(customerId).IsNotEmpty();
            Guard.That(billingCompanyId).IsNotEmpty();
            
            string emailBody;
            string emailSubject;
            string customerEmail;
            eventIntegrationService.Publish(new CustomerScrapeSessionFailed(customerId, billingCompanyId, errorNum)); 

            CustomerDto customer = customerByIdQuery.GetCustomerById(customerId);
            //Do billing compnay.... 

            switch (errorNum)
            {
                
                case ScrapingErrorResponseCodes.Unknown:
                case ScrapingErrorResponseCodes.UnhandledDataCondition:
                case ScrapingErrorResponseCodes.BrokenScript:
                    // Change Customer Status to Trying 
                    // Find out how to send an event to the customer to change status
                    // Notify Production Support
                    customerEmail = "Support@APS.co.za";
                    emailSubject = "Broken Script - Please Investigate."; // how do I get a billing company name here?
                    emailBody = "Hi\nBroken Script - Please investigate.";
                    break;


                // Invalid Credentials Error
                case ScrapingErrorResponseCodes.InvalidCredentials:

                    // Change Customer Status to Inactive
                    // Find out how to send an event to the customer to change status
                    // Notify customer via email
                    emailSubject = "Invalid Credentials on ??? "; // how do I get a billing company name here?
                   // emailBody = string.Format("Hi,{0} Your credentials were invalid on the {1} website.{0} Please Action. {0} Regards, {0}APS Team", Environment.NewLine, billingCompany.name); 
                    break;

                // Not signed up for e-Billing
                case ScrapingErrorResponseCodes.CustomerNotSignedUpForEBilling:

                    // Change Customer Status to Inactive
                    // Find out how to send an event to the customer to change status
                    // Notify customer via email
                    emailSubject = "Invalid Credentials on ??? "; // how do I get a billing company name here?
                    emailBody = "Hi,\n Your credentials were invalid on the {0} website.\n Please Action. \n Regards, \nAPS Team"; // add company to {0}
                    break;

                // Billing Company action required
                case ScrapingErrorResponseCodes.ActionRequiredbyBillingCompanyWebsite:

                    // Change Customer Status to Inactive
                    // Find out how to send an event to the customer to change status
                    // Notify customer via email
                    emailSubject = "Invalid Credentials on ??? "; // how do I get a billing company name here?
                    emailBody = "Hi,\n Your credentials were invalid on the {0} website.\n Please Action. \n Regards, \nAPS Team"; // add company to {0}
                    break;

                // Billing Company site down - Unscheduled maintenance
                case ScrapingErrorResponseCodes.BillingCompanySiteDown:

                    // Change Customer Status to Trying
                    // Find out how to send an event to the customer to change status
                    // Delay Scrape for a possibly determined timeframe
                    // How do I delay the scraper? send an event?
                    break;

                // Error Page Encountered
                case ScrapingErrorResponseCodes.ErrorPageEncountered:

                    // Change Customer Status to Trying
                    // Find out how to send an event to the customer to change status
                    // Delay Scrape for a short Time frame
                    // How do I delay the scraper? send an event?
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
