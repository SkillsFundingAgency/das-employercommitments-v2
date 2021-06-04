using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Service
{
    public class EmployerAccountsServiceTests
    {
        private Mock<IAccountApiClient> _accountApiClient;
        private EmployerAccountsService _employerAccountsService;
        private LegalEntityViewModel _legalEntityViewModel;
        private ResourceViewModel _resourceViewModel;
        private const string ExpectedAccountId = "ABC3421";

        public EmployerAccountsServiceTests()
        {
            _accountApiClient = new Mock<IAccountApiClient>();
        }

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();
            _accountApiClient.Setup(x => x.GetLegalEntitiesConnectedToAccount(ExpectedAccountId))
               .ReturnsAsync(new List<ResourceViewModel>());
            _employerAccountsService = new EmployerAccountsService(_accountApiClient.Object);
            _legalEntityViewModel = autoFixture.Create<LegalEntityViewModel>();
            _resourceViewModel = autoFixture.Create<ResourceViewModel>();
        }       


        [Test]
        public async Task Then_Api_GetLegalEntitiesForAccount_Is_Called()
        {
            //Arrange
            var expectedAccountId = "CDE3421";
            _accountApiClient.Setup(x => x.GetLegalEntitiesConnectedToAccount(expectedAccountId))
               .ReturnsAsync(new List<ResourceViewModel>());

            //Act
            await _employerAccountsService.GetLegalEntitiesForAccount(expectedAccountId);

            //Assert
            _accountApiClient.Verify(x => x.GetLegalEntitiesConnectedToAccount(expectedAccountId), Times.Once);
        }

        [Test]
        public async Task Then_Null_Is_Returned_If_No_LegalEntitiesExist_For_TheAccount()
        {   //Act
            var actual = await _employerAccountsService.GetLegalEntitiesForAccount(ExpectedAccountId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public async Task Then_Api_GetLegalEntity_Is_Called()
        {
            //Arrange
            _accountApiClient.Setup(x => x.GetLegalEntitiesConnectedToAccount(ExpectedAccountId))
                .ReturnsAsync(new List<ResourceViewModel>
                {  
                    new ResourceViewModel {Id = "4587"},
                    new ResourceViewModel {Id = "85214"}
                });
            _accountApiClient.Setup(x => x.GetLegalEntity(ExpectedAccountId, It.IsAny<long>()))
                .ReturnsAsync(_legalEntityViewModel);
                
            //Act
            var actual = await _employerAccountsService.GetLegalEntitiesForAccount(ExpectedAccountId);

            //Assert
            _accountApiClient.Verify(x => x.GetLegalEntity(ExpectedAccountId, It.IsAny<long>()), Times.Exactly(2));
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);
        }
    }
}
