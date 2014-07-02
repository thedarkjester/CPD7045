using System;
using System.Linq;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Aps.Fakes;
using Autofac;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aps.Shared.Tests.BillingCompanyTests
{
    [TestClass]
    public class BillingCompanyOpenClosedWindowTests
    {
        IContainer container;
        private BillingCompanyName companyName;
        private BillingCompanyType companyType;
        private BillingCompanyScrapingUrl companyUrl;

        [TestInitialize]
        public void Setup()
        {
            companyName = new BillingCompanyName("Company A");
            companyType = new BillingCompanyType(1);
            companyUrl = new BillingCompanyScrapingUrl("https://www.google.com/");

            //arrange
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<BillingCompanyRepositoryFake>().As<IBillingCompanyRepository>();
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>();

            container = builder.Build();
        }

        [TestMethod]
        public void Given_An_OpenClosedWindow_When_AddingToTheBillingCompany_TheItemsInsideIncrease()
        {
            //arrange
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            var openClosedWindow = new OpenClosedScrapingWindow(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), true, 2);
            //act
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedScrapingWindows.Count() == 1);
        }

        [TestMethod]
        public void Given_Multiple_OpenClosedWindow_When_AddingToTheBillingCompany_TheItemsInsideIncrease()
        {
            //arrange
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            var openClosedWindow = new OpenClosedScrapingWindow(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), true, 2);
            var openClosedWindow2 = new OpenClosedScrapingWindow(DateTime.Now.AddHours(3), DateTime.Now.AddHours(4), true, 2);

            //act
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow);
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow2);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedScrapingWindows.Count() == 2);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_Multiple_OpenClosedWindow_When_AddingToTheBillingCompanyLeftBasedOverlapsExist_ExceptionIsThrown()
        {
            //arrange
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues
                (companyName, companyType, companyUrl);
          
            var openClosedWindow = new OpenClosedScrapingWindow(DateTime.Now.AddHours(2), DateTime.Now.AddHours(4), true, 2);
            var openClosedWindow2 = new OpenClosedScrapingWindow(DateTime.Now.AddHours(1), DateTime.Now.AddHours(3), true, 2);

            //act
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow);
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow2);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedScrapingWindows.Count() == 2);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_Multiple_OpenClosedWindow_When_AddingToTheBillingCompanyRightBasedOverlapsExist_ExceptionIsThrown()
        {
            //arrange
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            var openClosedWindow = new OpenClosedScrapingWindow(DateTime.Now.AddHours(2), DateTime.Now.AddHours(4), true, 2);
            var openClosedWindow2 = new OpenClosedScrapingWindow(DateTime.Now.AddHours(3), DateTime.Now.AddHours(5), true, 2);

            //act
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow);
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow2);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedScrapingWindows.Count() == 2);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_Multiple_OpenClosedWindow_When_AddingToTheBillingCompanyMiddleBasedOverlapsExist_ExceptionIsThrown()
        {
            //arrange
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            var openClosedWindow = new OpenClosedScrapingWindow(DateTime.Now.AddHours(2), DateTime.Now.AddHours(5), true, 2);
            var openClosedWindow2 = new OpenClosedScrapingWindow(DateTime.Now.AddHours(3), DateTime.Now.AddHours(4), true, 2);

            //act
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow);
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow2);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedScrapingWindows.Count() == 2);
        }

        [TestMethod]
        public void Given_Multiple_OpenClosedWindows_When_RemovingFromTheBillingCompany_TheItemsInsideDecrease()
        {
            //arrange
            BillingCompanyFactory billingCompanyFactory = container.Resolve<BillingCompanyFactory>();
            BillingCompany billingCompany = billingCompanyFactory.ConstructBillingCompanyWithGivenValues(companyName, companyType, companyUrl);

            var openClosedWindow = new OpenClosedScrapingWindow(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), true, 2);
            var openClosedWindow2 = new OpenClosedScrapingWindow(DateTime.Now.AddHours(3), DateTime.Now.AddHours(4), true, 2);
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow);
            billingCompany.AddOpenClosedScrapingWindow(openClosedWindow2);

            //act
            billingCompany.RemoveOpenClosedWindow(openClosedWindow);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedScrapingWindows.Count() == 1);
        }

    }
}