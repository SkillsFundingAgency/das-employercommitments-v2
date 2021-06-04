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
    public class WhenMappingChooseOrganisationRequestToSelectLegalEntityViewModelTests
    {
        private Mock<IAccountApiClient> _accountApiClient;
        private ChooseOrganisationRequestToSelectLegalEntityViewModelMapper _mapper;
        private ChooseOrganisationRequest _chooseOrganisationRequest;
        private ResourceViewModel resourceViewModel;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();
            _accountApiClient = new Mock<IAccountApiClient>();
            _chooseOrganisationRequest = autoFixture.Create<ChooseOrganisationRequest>();
            resourceViewModel = autoFixture.Create<ResourceViewModel>();
            resourceViewModel.Id = "123";
            var legalEnityViewModel = autoFixture.Create<LegalEntityViewModel>();

            _accountApiClient.Setup(x => x.GetLegalEntitiesConnectedToAccount(_chooseOrganisationRequest.AccountHashedId))
                .ReturnsAsync(new List<ResourceViewModel> { resourceViewModel });

            _accountApiClient.Setup(x => x.GetLegalEntity(_chooseOrganisationRequest.AccountHashedId, Convert.ToInt64(resourceViewModel.Id)))
                .ReturnsAsync(legalEnityViewModel);

            _mapper = new ChooseOrganisationRequestToSelectLegalEntityViewModelMapper(_accountApiClient.Object);
        }

        [Test]
        public async Task Then_TransferConnectionCode_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_chooseOrganisationRequest);

            //Assert           
            Assert.AreEqual(_chooseOrganisationRequest.transferConnectionCode, result.TransferConnectionCode);
        }

        [Test]
        public async Task Then_CohortRef_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_chooseOrganisationRequest);

            //Assert           
            Assert.AreEqual(_chooseOrganisationRequest.cohortRef, result.CohortRef);
        }


        [Test]
        public async Task Then_LegalEntitiy_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_chooseOrganisationRequest);

            //Assert           
            Assert.AreEqual(1, result.LegalEntities.Count());
        }

        [Test]
        public async Task Then_GetLegalEntitiesForAccount_Is_Called()
        {
            //Act
            var result = await _mapper.Map(_chooseOrganisationRequest);

            //Assert
            _accountApiClient.Verify(x => x.GetLegalEntitiesConnectedToAccount(_chooseOrganisationRequest.AccountHashedId),
                Times.Once);
        }

        [Test]
        public async Task Then_GetLegalEntity_Is_Called()
        {
            //Act
            var result = await _mapper.Map(_chooseOrganisationRequest);

            //Assert
            _accountApiClient.Verify(x => x.GetLegalEntity(_chooseOrganisationRequest.AccountHashedId, Convert.ToInt64(resourceViewModel.Id)),
                Times.Once);
        }

    }
}
