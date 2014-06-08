using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Caliburn.Micro;
using Aps.Scraping;
using Aps.Fakes;
using Aps.Integration.EnumTypes;

namespace Aps.Shared.Tests.CoreTests
{
    [TestClass]
    public class ScrapingObjectTest
    {

        IContainer container;
        private Guid billingCompanyId;
        private Guid customerId;

     //   public string scrapeType;
        public string scrapeStatus;

        public DateTime createdDate;
        public DateTime ScheduledDate;

        ScrapeSessionTypes scrapeSessionTypes;

       // public bool registrationType;

        [TestInitialize]
        public void Setup()
        {
            customerId = Guid.NewGuid();
            billingCompanyId = Guid.NewGuid();
            scrapeSessionTypes = ScrapeSessionTypes.CrossCheckScrapper;
            


            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<ScrapingObjectRepositoryFake>().As<IScrapingObjectRepository>();
            builder.RegisterType<ScrapingObjectCreator>().As<ScrapingObjectCreator>();

            container = builder.Build();
        }


        [TestMethod]
        public void TestConstructionOfNewScrapingObject()
        {
            // act
            ScrapingObject scrapingObject = container.Resolve<IScrapingObjectRepository>().BuildNewScrapingObject(customerId, billingCompanyId, scrapeSessionTypes);

            Assert.IsTrue(scrapingObject.billingCompanyId == billingCompanyId);
            Assert.IsTrue(scrapingObject.customerId == customerId);
            Assert.IsTrue(scrapingObject.scrapeSessionTypes == scrapeSessionTypes);

           // Assert.IsTrue(scrapingObject.scrapeType == "Register");
            // Assert.IsTrue(scrapingObject.scrapeStatus == "Active");

        }
    }
}
