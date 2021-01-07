using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class WhatIsTheNewStartDateViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        private GetApprenticeshipResponse _apprenticeshipResponse;

        private WhatIsTheNewStartDateViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            var _autoFixture = new Fixture();

            _apprenticeshipResponse = _autoFixture.Build<GetApprenticeshipResponse>().Create();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockCommitmentsApiClient.Setup(m => m.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_apprenticeshipResponse);

            _mapper = new WhatIsTheNewStartDateViewModelMapper(_mockCommitmentsApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(WhatIsTheNewStartDateRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(WhatIsTheNewStartDateRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ProviderId_IsMapped(WhatIsTheNewStartDateRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.ProviderId, result.ProviderId);
        }

        [Test, MoqAutoData]
        public async Task WhenRequestingTheWhatIsTheNewStartDatePage_ThenTheGetApprenticeshipIsCalled(WhatIsTheNewStartDateRequest request)
        {
            var result = await _mapper.Map(request);

            _mockCommitmentsApiClient.Verify(m => m.GetApprenticeship(request.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test, MoqAutoData]
        public async Task StopDate_IsMapped(WhatIsTheNewStartDateRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(_apprenticeshipResponse.StopDate, result.StopDate);
        }

        [Test, MoqAutoData]
        public async Task ProviderName_IsMapped(WhatIsTheNewStartDateRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(_apprenticeshipResponse.ProviderName, result.ProviderName);
        }
    }
}
