using System;
using System.Linq;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
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
            builder.RegisterType<BillingCompanyRepositoryFake>().As<BillingCompanyRepositoryFake>();
            builder.RegisterType<BillingCompanyCreator>().As<BillingCompanyCreator>();

            container = builder.Build();
        }

        [TestMethod]
        public void Given_An_OpenClosedWindow_When_AddingToTheBillingCompany_TheItemsInsideIncrease()
        {
            //arrange
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().BuildNewBillingCompany(companyName, companyType, companyUrl);
            var openClosedWindow = new OpenClosedWindow(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), true, 2);
            //act
            billingCompany.AddOpenClosedWindow(openClosedWindow);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedWindows.Count() == 1);
        }

        [TestMethod]
        public void Given_Multiple_OpenClosedWindow_When_AddingToTheBillingCompany_TheItemsInsideIncrease()
        {
            //arrange
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().BuildNewBillingCompany(companyName, companyType, companyUrl);
            var openClosedWindow = new OpenClosedWindow(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), true, 2);
            var openClosedWindow2 = new OpenClosedWindow(DateTime.Now.AddHours(3), DateTime.Now.AddHours(4), true, 2);

            //act
            billingCompany.AddOpenClosedWindow(openClosedWindow);
            billingCompany.AddOpenClosedWindow(openClosedWindow2);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedWindows.Count() == 2);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_Multiple_OpenClosedWindow_When_AddingToTheBillingCompanyLeftBasedOverlapsExist_ExceptionIsThrown()
        {
            //arrange
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().BuildNewBillingCompany(companyName, companyType, companyUrl);
          
            var openClosedWindow = new OpenClosedWindow(DateTime.Now.AddHours(2), DateTime.Now.AddHours(4), true, 2);
            var openClosedWindow2 = new OpenClosedWindow(DateTime.Now.AddHours(1), DateTime.Now.AddHours(3), true, 2);

            //act
            billingCompany.AddOpenClosedWindow(openClosedWindow);
            billingCompany.AddOpenClosedWindow(openClosedWindow2);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedWindows.Count() == 2);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_Multiple_OpenClosedWindow_When_AddingToTheBillingCompanyRightBasedOverlapsExist_ExceptionIsThrown()
        {
            //arrange
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().BuildNewBillingCompany(companyName, companyType, companyUrl);

            var openClosedWindow = new OpenClosedWindow(DateTime.Now.AddHours(2), DateTime.Now.AddHours(4), true, 2);
            var openClosedWindow2 = new OpenClosedWindow(DateTime.Now.AddHours(3), DateTime.Now.AddHours(5), true, 2);

            //act
            billingCompany.AddOpenClosedWindow(openClosedWindow);
            billingCompany.AddOpenClosedWindow(openClosedWindow2);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedWindows.Count() == 2);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void Given_Multiple_OpenClosedWindow_When_AddingToTheBillingCompanyMiddleBasedOverlapsExist_ExceptionIsThrown()
        {
            //arrange
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().BuildNewBillingCompany(companyName, companyType, companyUrl);

            var openClosedWindow = new OpenClosedWindow(DateTime.Now.AddHours(2), DateTime.Now.AddHours(5), true, 2);
            var openClosedWindow2 = new OpenClosedWindow(DateTime.Now.AddHours(3), DateTime.Now.AddHours(4), true, 2);

            //act
            billingCompany.AddOpenClosedWindow(openClosedWindow);
            billingCompany.AddOpenClosedWindow(openClosedWindow2);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedWindows.Count() == 2);
        }

        [TestMethod]
        public void Given_Multiple_OpenClosedWindows_When_RemovingFromTheBillingCompany_TheItemsInsideDecrease()
        {
            //arrange
            BillingCompany billingCompany = container.Resolve<BillingCompanyRepositoryFake>().BuildNewBillingCompany(companyName, companyType, companyUrl);
            var openClosedWindow = new OpenClosedWindow(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), true, 2);
            var openClosedWindow2 = new OpenClosedWindow(DateTime.Now.AddHours(3), DateTime.Now.AddHours(4), true, 2);
            billingCompany.AddOpenClosedWindow(openClosedWindow);
            billingCompany.AddOpenClosedWindow(openClosedWindow2);

            //act
            billingCompany.RemoveOpenClosedWindow(openClosedWindow);

            //assert
            Assert.IsTrue(billingCompany.OpenClosedWindows.Count() == 1);
        }

    }
}