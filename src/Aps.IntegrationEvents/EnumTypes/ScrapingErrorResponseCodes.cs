using System.ComponentModel;

namespace Aps.Integration.EnumTypes
{
    public enum ScrapingErrorResponseCodes
    {
        Unknown = 0,
        [Description("Invalid Credentials")]
        InvalidCredentials = 1,
        [Description("Customer Not Signed Up for e-Billing")]
        CustomerNotSignedUpForEBilling = 2,
        [Description("Action Required by Billing Company’s Website")]
        ActionRequiredbyBillingCompanyWebsite = 3,
        [Description("Billing Company’s Site Down")]
        BillingCompanySiteDown = 4,
        [Description("Error Page Encountered")]
        ErrorPageEncountered = 5,
        [Description("Broken Script")]
        BrokenScript = 6
    }
}