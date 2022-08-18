using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Encoding;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    public class WhenMappingAgreementNotSignedViewModelMapperTests
    {
        private Mock<IEmployerAccountsService> _employerAccountsService;
        private Mock<IEncodingService> _encodingService;
        private Account _account;
        private LegalEntitySignedAgreementViewModelToAgreementNotSignedViewModelMapper _mapper;
        private LegalEntitySignedAgreementViewModel _legalEntitySignedAgreementViewModel;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();
            _employerAccountsService = new Mock<IEmployerAccountsService>();
            _encodingService = new Mock<IEncodingService>();
            _legalEntitySignedAgreementViewModel = autoFixture.Create<LegalEntitySignedAgreementViewModel>();

            _account = autoFixture.Create<Account>();
            _account.Id = 123;

            _encodingService.Setup(x => x.Decode(It.IsAny<string>(), EncodingType.AccountId)).Returns(123);
            _employerAccountsService.Setup(x => x.GetAccount(123)).ReturnsAsync(_account);            

            _mapper = new LegalEntitySignedAgreementViewModelToAgreementNotSignedViewModelMapper(_employerAccountsService.Object, _encodingService.Object);
        }

        [TestCase(ApprenticeshipEmployerType.Levy , true)]
        [TestCase(ApprenticeshipEmployerType.NonLevy, false)]
        public async Task Then_CanContinueAnyway_Is_Mapped(ApprenticeshipEmployerType apprenticeshipEmployerType, bool canContinue)
        {
            //Arrange
            _account.ApprenticeshipEmployerType = apprenticeshipEmployerType;
            
            //Act
            var result = await _mapper.Map(_legalEntitySignedAgreementViewModel);

            //Assert           
            Assert.AreEqual(canContinue, result.CanContinueAnyway);
        }

        [Test]
        public async Task Then_AccountLegalEntityPublicHashedId_Is_Mapped()
        {          
            //Act
            var result = await _mapper.Map(_legalEntitySignedAgreementViewModel);

            //Assert           
            Assert.AreEqual(_legalEntitySignedAgreementViewModel.AccountLegalEntityHashedId, result.AccountLegalEntityHashedId);
        }

        [Test]
        public async Task Then_LegalEntityName_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_legalEntitySignedAgreementViewModel);

            //Assert           
            Assert.AreEqual(_legalEntitySignedAgreementViewModel.LegalEntityName, result.LegalEntityName);
        }

        [Test]
        public async Task Then_LegalEntityCode_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_legalEntitySignedAgreementViewModel);

            //Assert           
            Assert.AreEqual(_legalEntitySignedAgreementViewModel.AccountLegalEntityId, result.AccountLegalEntityId);
        }

        [Test]
        public async Task Then_TransferConnectionCode_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_legalEntitySignedAgreementViewModel);

            //Assert           
            Assert.AreEqual(_legalEntitySignedAgreementViewModel.TransferConnectionCode, result.TransferConnectionCode);
        }
    }
}
