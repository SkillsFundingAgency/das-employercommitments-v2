using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    public class WhenMappingSelectedLegalEntityToSignedAgreementViewModelTests
    {
        private Mock<IAccountApiClient> _accountApiClient;
        private SelectedLegalEntityToSignedAgreementViewModelMapper _mapper;
        private SelectLegalEntityViewModel _selectLegalEntityViewModel;
        private LegalEntitySignedAgreementViewModel _legalEntitySignedAgreementViewModel;
        private ResourceViewModel _resourceViewModel;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();
            _accountApiClient = new Mock<IAccountApiClient>();
            _selectLegalEntityViewModel = autoFixture.Create<SelectLegalEntityViewModel>();            
            _legalEntitySignedAgreementViewModel = autoFixture.Create<LegalEntitySignedAgreementViewModel>();
            _resourceViewModel = autoFixture.Create<ResourceViewModel>();
            _resourceViewModel.Id = "123";
            var legalEnityViewModel = autoFixture.Create<LegalEntityViewModel>();
            legalEnityViewModel.Code = _selectLegalEntityViewModel.LegalEntityCode;

            _accountApiClient.Setup(x => x.GetLegalEntitiesConnectedToAccount(_selectLegalEntityViewModel.AccountHashedId))
                .ReturnsAsync(new List<ResourceViewModel> { _resourceViewModel });

            _accountApiClient.Setup(x => x.GetLegalEntity(_selectLegalEntityViewModel.AccountHashedId, Convert.ToInt64(_resourceViewModel.Id)))
                .ReturnsAsync(legalEnityViewModel);

            _mapper = new SelectedLegalEntityToSignedAgreementViewModelMapper(_accountApiClient.Object);
        }

        [Test]
        public async Task Then_TransferConnectionCode_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_selectLegalEntityViewModel);

            //Assert           
            Assert.AreEqual(_selectLegalEntityViewModel.TransferConnectionCode, result.TransferConnectionCode);
        }

        [Test]
        public async Task Then_LegalEntityCode_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_selectLegalEntityViewModel);

            //Assert           
            Assert.AreEqual(_selectLegalEntityViewModel.LegalEntityCode, result.LegalEntityCode);
        }

        [Test]
        public async Task Then_CohortRef_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_selectLegalEntityViewModel);

            //Assert           
            Assert.AreEqual(_selectLegalEntityViewModel.CohortRef, result.CohortRef);
        }


        [Test]
        public async Task Then_GetLegalEntitiesForAccount_Is_Called()
        {
            //Act
            var result = await _mapper.Map(_selectLegalEntityViewModel);

            //Assert
            _accountApiClient.Verify(x => x.GetLegalEntitiesConnectedToAccount(_selectLegalEntityViewModel.AccountHashedId),
                Times.Once);
        }

        [Test]
        public async Task Then_GetLegalEntity_Is_Called()
        {
            //Act
            var result = await _mapper.Map(_selectLegalEntityViewModel);

            //Assert
            _accountApiClient.Verify(x => x.GetLegalEntity(_selectLegalEntityViewModel.AccountHashedId, Convert.ToInt64(_resourceViewModel.Id)),
                Times.Once);
        }

    }
}
