using System;
using Seterlund.CodeGuard;

namespace Aps.BillingCompanies.ValueObjects
{
    public class BillingCompanyScrapingUrl
    {
        private readonly string url;

        protected BillingCompanyScrapingUrl()
        {

        }

         public string ScrapingUrl
         {
             get { return url; }
         }

        public BillingCompanyScrapingUrl(string url)
        {
            Guard.That(url).IsNotEmpty();
            Guard.That(url).IsTrue(s => s.IndexOf("http://", System.StringComparison.InvariantCultureIgnoreCase) == -1, "https is missing");

            Uri testUri;
            Uri.TryCreate(url, UriKind.Absolute, out testUri);

            if (testUri == null)
            {
                throw new ArgumentException();
            }

            if (url.IndexOf("http", System.StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                this.url = "https://" + url;
            }
            else
            {
                this.url = url;
            }
        }

        public BillingCompanyScrapingUrl ChangeUrl(string newUrl)
        {
            return new BillingCompanyScrapingUrl(newUrl);
        }

        public override string ToString()
        {
            return url;
        }
    }
}