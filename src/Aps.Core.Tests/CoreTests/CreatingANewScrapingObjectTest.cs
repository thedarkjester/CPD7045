using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Aps.Core;
using Caliburn.Micro;

namespace Aps.Shared.Tests.CoreTests
{
    [TestClass]
    public class CreatingANewScrapingObjectTest
    {

        IContainer container;
        private Guid billingCompanyId;
        private string URL;
        private Guid customerId;
        private string plainText;
        private string hiddenText;
        private DateTime testDate;

        [TestInitialize]
        public void Setup()
        {
            customerId = Guid.NewGuid();
            billingCompanyId = Guid.NewGuid();

            URL = "www.website.co.za";
            plainText = "ViljoeW2";
            hiddenText = "Telkom123";
            testDate = DateTime.Now;

            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<ScrapingObjectRepositoryFake>().As<ScrapingObjectRepositoryFake>();
            builder.RegisterType<ScrapingObjectCreator>().As<ScrapingObjectCreator>();

            container = builder.Build();
        }

        [TestMethod]
        public void CreatingANewScrapingObject()
        {
            // arrange

            // act
            ScrapingObject scrapingObject = container.Resolve<ScrapingObjectRepositoryFake>().BuildNewScrapingObject(customerId, billingCompanyId, URL, plainText, hiddenText);

            // assert

            Assert.IsTrue(scrapingObject.billingCompanyId == billingCompanyId);
            Assert.IsTrue(scrapingObject.customerId == customerId);
            Assert.IsTrue(scrapingObject.URL == URL);
            Assert.IsTrue(scrapingObject.plainText1 == plainText);
            Assert.IsTrue(scrapingObject.hiddenText1 == hiddenText);

            Assert.IsTrue(scrapingObject.scrapeType == "Register");
            Assert.IsTrue(scrapingObject.scrapeStatus == "Active");
            Assert.IsTrue(scrapingObject.timeToRun.Date == testDate.Date);
            Assert.IsTrue(scrapingObject.timeCreated.Date == testDate.Date);
        }
    }
}
