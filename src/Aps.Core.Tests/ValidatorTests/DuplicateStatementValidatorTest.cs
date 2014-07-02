using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Aps.AccountStatements;
using Aps.Scheduling.ApplicationService.Validation;
using Aps.Scheduling.ApplicationService;
using System.Collections.Generic;

namespace Aps.Domain.Tests.ValidatorTests
{
    [TestClass]
    public class DuplicateStatementValidatorTest
    {

        Mock<IAccountStatementRepository> mockAccountStatementRepository;
        Guid customerId;
        Guid billingCompanyId;

         [TestInitialize]
        public void Setup()
        {
            billingCompanyId = Guid.NewGuid();
            customerId = Guid.NewGuid();
            mockAccountStatementRepository = new Mock<IAccountStatementRepository>();
            
        }


        [ExpectedException(typeof(DuplicateStatementException))]
        [TestMethod]
        public void Given_CustomerIdBillingCompanyIdStatementDate_When_CheckIfStatementExists_Then_ReturnTrue_And_ThrowException()
        {
            //arrange
            DateTime date = DateTime.Now.AddDays(-10);
            mockAccountStatementRepository.Setup(x => x.AccountStatementExistsForCustomer(customerId,billingCompanyId, date)).Returns(true);
            List<InterpretedScrapeSessionDataPair> list = new List<InterpretedScrapeSessionDataPair> { new InterpretedScrapeSessionDataPair { Id = 1, keyValuePair = new KeyValuePair<string, object>("Statement Date", date) } };
     
            //act
            DuplicateStatementValidator duplicateStatementValidator = new DuplicateStatementValidator(mockAccountStatementRepository.Object);
            duplicateStatementValidator.Validate(list, customerId, billingCompanyId);

            //assert throw exception

        }

        [TestMethod]
        public void Given_CustomerIdBillingCompanyIdStatementDate_When_CheckIfStatementExists_Then_ReturnFalse()
        {
            //arrange
            DateTime date = DateTime.Now;
            mockAccountStatementRepository.Setup(x => x.AccountStatementExistsForCustomer(customerId, billingCompanyId, date)).Returns(false);
            List<InterpretedScrapeSessionDataPair> list = new List<InterpretedScrapeSessionDataPair> { new InterpretedScrapeSessionDataPair { Id = 1, keyValuePair = new KeyValuePair<string, object>("Statement Date", date) } };
            
            //act
            DuplicateStatementValidator duplicateStatementValidator = new DuplicateStatementValidator(mockAccountStatementRepository.Object);
            duplicateStatementValidator.Validate(list, customerId, billingCompanyId);
            
            //assert (verify)
            mockAccountStatementRepository.Verify();
        }
    }
}
