using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    public class WhenMappingChooseOrganisationRequestToSelectLegalEntityViewModelTests
    {       
        private Mock<IEmployerAccountsService> _employerAccountsService;
        private SelectLegalEntityRequestToSelectLegalEntityViewModelMapper _mapper;
        private SelectLegalEntityRequest _chooseOrganisationRequest;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();
            _employerAccountsService = new Mock<IEmployerAccountsService>();
            _chooseOrganisationRequest = autoFixture.Create<SelectLegalEntityRequest>();          
            var legalEntity = autoFixture.Create<LegalEntity>();
            _employerAccountsService.Setup(x => x.GetLegalEntitiesForAccount(_chooseOrganisationRequest.AccountHashedId))
                .ReturnsAsync(new List<LegalEntity> { legalEntity });
            
            _mapper = new SelectLegalEntityRequestToSelectLegalEntityViewModelMapper(_employerAccountsService.Object);
        }

        [Test]
        public async Task Then_TransferConnectionCode_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_chooseOrganisationRequest);

            //Assert           
            Assert.That(result.TransferConnectionCode, Is.EqualTo(_chooseOrganisationRequest.transferConnectionCode));
        }

        [Test]
        public async Task Then_CohortRef_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_chooseOrganisationRequest);

            //Assert           
            Assert.That(result.CohortRef, Is.EqualTo(_chooseOrganisationRequest.cohortRef));
        }


        [Test]
        public async Task Then_LegalEntitiy_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_chooseOrganisationRequest);

            //Assert           
            Assert.That(result.LegalEntities.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Then_GetLegalEntitiesForAccount_Is_Called()
        {
            //Act
            var result = await _mapper.Map(_chooseOrganisationRequest);

            //Assert
            _employerAccountsService.Verify(x => x.GetLegalEntitiesForAccount(_chooseOrganisationRequest.AccountHashedId),
                Times.Once);
        }
    }
}
