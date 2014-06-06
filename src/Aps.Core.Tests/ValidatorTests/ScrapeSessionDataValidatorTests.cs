using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aps.Core.Validation;
using Aps.Core;

namespace Aps.Shared.Tests.ValidatorTests
{
    [TestClass]
    public class ScrapeSessionDataValidatorTests
    {
        [TestMethod]
        public void WhenReceicingScrapeSessionDataValidationResultsShouldBeReturned()
        {
            ScrapeSessionDataValidator scrapeSessionDataValidator = new ScrapeSessionDataValidator();
            ScrapeSessionData scrapeSessionData = new ScrapeSessionData();
            ValidationResults vResults = scrapeSessionDataValidator.validateScrapeData(scrapeSessionData);
            if (vResults.IsValid)
            {
                //successful validation with no validation errors
            }
            else
            {
                foreach (var vResult in vResults.Results)
                {
                    //check which datapairs validated insuccessfully and their error codes.
                    
                }
            }
        }
    }
}
