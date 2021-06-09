using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    public class WhenMappingSelectedLegalEntityToSignedAgreementViewModelTests
    {        
        private Mock<IEmployerAccountsService> _employerAccountsService;
        private SelectedLegalEntityToSignedAgreementViewModelMapper _mapper;
        private SelectLegalEntityViewModel _selectLegalEntityViewModel;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();
            _employerAccountsService = new Mock<IEmployerAccountsService>();
            _selectLegalEntityViewModel = autoFixture.Create<SelectLegalEntityViewModel>();
            
            var legalEntity = autoFixture.Create<LegalEntity>();
            legalEntity.Id = _selectLegalEntityViewModel.LegalEntityId;
            _employerAccountsService.Setup(x => x.GetLegalEntitiesForAccount(_selectLegalEntityViewModel.AccountHashedId))
                .ReturnsAsync(new List<LegalEntity> { legalEntity });

            _mapper = new SelectedLegalEntityToSignedAgreementViewModelMapper(_employerAccountsService.Object);
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
            Assert.AreEqual(_selectLegalEntityViewModel.LegalEntityId, result.LegalEntityId);
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
            _employerAccountsService.Verify(x => x.GetLegalEntitiesForAccount(_selectLegalEntityViewModel.AccountHashedId),
                Times.Once);
        }   

    }
}
