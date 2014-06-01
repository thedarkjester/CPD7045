using System;
using System.Text;
using System.Threading.Tasks;

namespace Aps.Core
{
    public class ScrapingObject
    {
        public Guid customerId;
        public Guid billingCompanyId;
        public readonly string URL;
        public readonly string plainText1;
        public readonly string hiddenText1;

        public string scrapeType;
        public string scrapeStatus;
        public DateTime timeToRun;
        public DateTime timeCreated;

        public ScrapingObject(Guid customerId, Guid billingCompanyId, string URL, string plainText1, string hiddenText1)
        {
            this.customerId = customerId;
            this.billingCompanyId = billingCompanyId;
            this.URL = URL;
            this.plainText1 = plainText1;
            this.hiddenText1 = hiddenText1;

            scrapeType = "Register";
            scrapeStatus = "Active";

            timeToRun = DateTime.Now;
            timeCreated = DateTime.Now;

        }
    }
}