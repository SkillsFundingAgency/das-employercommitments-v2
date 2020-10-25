using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class EnterNewTrainingProviderViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private Mock<IEncodingService> _mockEncodingService;

        private EnterNewTrainingProviderViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockEncodingService = new Mock<IEncodingService>();



            _mapper = new EnterNewTrainingProviderViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(EnterNewTrainingProviderRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(EnterNewTrainingProviderRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task Ukprn_IsMapped(EnterNewTrainingProviderRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.Ukprn, result.Ukprn);
        }

        [Test]
        public async Task WhenRequestingEnterNewTrainingProvider_ThenAccountIdIsDecodedOnce()
        {


        }
    }
}
