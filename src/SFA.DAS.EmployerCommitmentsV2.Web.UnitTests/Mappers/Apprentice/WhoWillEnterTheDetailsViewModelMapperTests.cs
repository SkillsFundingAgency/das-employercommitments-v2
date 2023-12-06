using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class WhoWillEnterTheDetailsViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        private GetProviderResponse _getProviderResponse;

        private WhoWillEnterTheDetailsViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _getProviderResponse = MockGetProvider();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockCommitmentsApiClient.Setup(m => m.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getProviderResponse);

            _mapper = new WhoWillEnterTheDetailsViewModelMapper(_mockCommitmentsApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(ChangeOfProviderRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(ChangeOfProviderRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
        }

        [Test, MoqAutoData]
        public async Task ProviderId_IsMapped(ChangeOfProviderRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.That(result.ProviderId, Is.EqualTo(request.ProviderId));
        }

        [Test, MoqAutoData]
        public async Task WhenRequestingTheWhoWillEnterTheDetailsPage_ThenTheGetProviderRequestIsCalled(ChangeOfProviderRequest request)
        {
            var result = await _mapper.Map(request);

            _mockCommitmentsApiClient.Verify(m => m.GetProvider(request.ProviderId.Value, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test, MoqAutoData]
        public async Task WhenRequestingTheWhoWillEnterTheDetailsPage_ThenTheProviderNameIsReturnedAndMapped(ChangeOfProviderRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.That(_getProviderResponse.Name, Is.EqualTo(result.ProviderName));
        }
        
        private GetProviderResponse MockGetProvider()
        {
            return new GetProviderResponse
            {
                Name = "Test Provider",
                ProviderId = 12345678
            };
        }
    }
}
