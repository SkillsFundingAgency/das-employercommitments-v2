using System;
using System.Data;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ChangeProviderRequestedConfirmationViewModelMapperTests
    {

        private Mock<ICommitmentsApiClient> _mockCommitmentsApi;
        private GetApprenticeshipResponse _apprenticeshipResponse;
        private GetProviderResponse _providerResponse;

        private ChangeProviderRequestedConfirmationViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();

            _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
                                        .With(a => a.FirstName, "FirstName")
                                        .With(a => a.LastName, "LastName")
                                        .Create();

            _providerResponse = autoFixture.Build<GetProviderResponse>()
                                        .With(p => p.Name, "Test Provider")
                                        .Create();

            _mockCommitmentsApi = new Mock<ICommitmentsApiClient>();

            _mockCommitmentsApi.Setup(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_apprenticeshipResponse);
            _mockCommitmentsApi.Setup(c => c.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_providerResponse);

            _mapper = new ChangeProviderRequestedConfirmationViewModelMapper(_mockCommitmentsApi.Object);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
        }

        [Test, MoqAutoData]
        public async Task GetApprenticeshipIsCalled(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            _mockCommitmentsApi.Verify(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeName_IsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.That(result.ApprenticeName, Is.EqualTo($"{_apprenticeshipResponse.FirstName} {_apprenticeshipResponse.LastName}"));
        }

        [Test, MoqAutoData]
        public async Task ProviderName_IsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.That(result.ProviderName, Is.EqualTo(_providerResponse.Name));
        }

        [Test, MoqAutoData]
        public async Task ProviderAddDetails_IsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.That(result.ProviderAddDetails, Is.EqualTo(request.ProviderAddDetails));
        }

        [Test, MoqAutoData]
        public async Task StoppedDuringCoP_IsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.That(result.StoppedDuringCoP, Is.EqualTo(request.StoppedDuringCoP));
        }

    }
}
