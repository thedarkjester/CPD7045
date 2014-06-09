using System;
using Aps.Integration;
using Caliburn.Micro;
using Seterlund.CodeGuard;
using Aps.Integration.EnumTypes;
using Aps.Integration.Queries.CustomerQueries;
using Aps.Integration.Queries.CustomerQueries.Dtos;
using Aps.Integration.Queries.BillingCompanyQueries;
using Aps.Integration.Queries.BillingCompanyQueries.Dtos;
using Aps.Integration.Events;

namespace Aps.Scheduling.ApplicationService
{
    public class FailureHandler
    {
        private readonly EventIntegrationService eventIntegrationService;
        private readonly IEventAggregator eventAggregator;
        private readonly CustomerByIdQuery customerByIdQuery;
        private readonly BillingCompanyByIdQuery billingCompanyByIdQuery;

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
            string newStatus = null;

            CustomerDto customer = customerByIdQuery.GetCustomerById(customerId);
            BillingCompanyDto billingCompany = billingCompanyByIdQuery.GetBillingCompanyById(billingCompanyId);
            
            switch (errorNum)
            {
                //Unknown errors and Broken Script errors
                case ScrapingErrorResponseCodes.Unknown:
                case ScrapingErrorResponseCodes.UnhandledDataCondition:
                case ScrapingErrorResponseCodes.BrokenScript:
                    // Change Customer Status to Trying 
                    // Find out how to send an event to the customer to change status
                    newStatus = "Trying";
                    // Notify Production Support
                    customerEmail = "Support@APS.co.za";
                    emailSubject = "Broken Script - Please Investigate.";
                    emailBody = string.Format("Hi {0} Broken Script - Please investigate. Error Code = {1}", Environment.NewLine, errorNum);
                    break;


                // Invalid Credentials Error
                case ScrapingErrorResponseCodes.InvalidCredentials:

                    // Change Customer Status to Inactive
                    // Find out how to send an event to the customer to change status
                    newStatus = "Inactive";
                    // Notify customer via email
                    customerEmail = customer.Email;
                    emailSubject = string.Format("Invalid Credentials on {0}", billingCompany.Name); 
                    emailBody = string.Format("Hi,{0} Your credentials were invalid on the {1} website.{0} Please Action. {0} Regards, {0}APS Team", Environment.NewLine, billingCompany.Name); 
                    break;

                // Not signed up for e-Billing
                case ScrapingErrorResponseCodes.CustomerNotSignedUpForEBilling:

                    // Change Customer Status to Inactive
                    // Find out how to send an event to the customer to change status
                    newStatus = "Inactive";
                    // Notify customer via email
                    customerEmail = customer.Email;
                    emailSubject = string.Format("Not signed-up for e-Billing on {0}", billingCompany.Name); 
                    emailBody = string.Format("Hi, {0}Your {1} account is not signed-up for e-Billing. {0}Please take the necessary actions on your account. {0}Regards, {0}APS Team", Environment.NewLine, billingCompany.Name); 
                    break;

                // Billing Company action required
                case ScrapingErrorResponseCodes.ActionRequiredbyBillingCompanyWebsite:

                    // Change Customer Status to Inactive
                    // Find out how to send an event to the customer to change status
                    newStatus = "Inactive";
                    // Notify customer via email
                    customerEmail = customer.Email;
                    emailSubject = string.Format("Action required on {0} website", billingCompany.Name); 
                    emailBody = string.Format("Hi, {0}Your {1} account is needing action. {0}Please take the necessary actions on your account. {0}Regards, {0}APS Team", Environment.NewLine, billingCompany.Name); 
                    break;

                // Billing Company site down - Unscheduled maintenance
                case ScrapingErrorResponseCodes.BillingCompanySiteDown:

                    // Change Customer Status to Trying
                    // Find out how to send an event to the customer to change status
                    newStatus = "Trying";
                    // Delay Scrape for a possibly determined timeframe
                    // How do I delay the scraper? send an event?
                    break;

                // Error Page Encountered
                case ScrapingErrorResponseCodes.ErrorPageEncountered:

                    // Change Customer Status to Trying
                    // Find out how to send an event to the customer to change status
                    newStatus = "Trying";
                    // Delay Scrape for a short Time frame
                    // How do I delay the scraper? send an event?
                    break;

            }

            CustomerScrapeSessionFailed errorEvent = new CustomerScrapeSessionFailed(customerId, billingCompanyId, errorNum, newStatus);

            eventAggregator.Publish(errorEvent);
            eventIntegrationService.Publish(errorEvent);

        }

        public void SendMail(String CustomerEmail, String emailSubject, String emailBody)
        {
            EmailMessage mail = new EmailMessage();

            mail.SendEmail(CustomerEmail, emailSubject, emailBody);

        }

    }
}
