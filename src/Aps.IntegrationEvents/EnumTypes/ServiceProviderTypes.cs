using System.ComponentModel;

namespace Aps.Integration.EnumTypes
{
    public enum ServiceProviderTypes
    {
        Uknown = 0,
        [Description("Minicipal Account")]
        MinicipalAccount = 1,
        [Description("Credit Card Provider")]
        CreditCardProvider = 2,
        [Description("Tele Communication Provider")]
        TeleCommunicationProvider = 3
    }
}