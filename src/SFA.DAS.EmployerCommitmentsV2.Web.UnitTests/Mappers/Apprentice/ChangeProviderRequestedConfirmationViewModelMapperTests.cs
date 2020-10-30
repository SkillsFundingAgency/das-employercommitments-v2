using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ChangeProviderRequestedConfirmationViewModelMapperTests
    {

        private Mock<ICommitmentsApiClient> _mockCommitmentsApi;
        private Mock<IEncodingService> _mockEncodingService;
        private GetApprenticeshipResponse _apprenticeshipResponse;

        private ChangeProviderRequestedConfirmationViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();

            _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
                                        .With(a => a.FirstName, "FirstName")
                                        .With(a => a.LastName, "LastName")
                                        .Create();

            _mockCommitmentsApi = new Mock<ICommitmentsApiClient>();
            _mockEncodingService = new Mock<IEncodingService>();

            _mockCommitmentsApi.Setup(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_apprenticeshipResponse);

            _mapper = new ChangeProviderRequestedConfirmationViewModelMapper(_mockCommitmentsApi.Object, _mockEncodingService.Object);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }
       

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedIdIsDecoded(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            _mockEncodingService.Verify(d => d.Decode(It.IsAny<string>(), EncodingType.ApprenticeshipId), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetApprenticeshipIsCalled(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            _mockCommitmentsApi.Verify(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeNameIsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual($"{_apprenticeshipResponse.FirstName} {_apprenticeshipResponse.LastName}", result.ApprenticeName);
        }

    }
}
