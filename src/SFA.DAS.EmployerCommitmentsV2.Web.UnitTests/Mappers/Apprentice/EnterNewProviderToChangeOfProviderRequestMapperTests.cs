

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
    public class EnterNewProviderToChangeOfProviderRequestMapperTests
    {
        private GetProviderResponse _getProviderResponse;
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private EnterNewTrainingProviderToChangeOfProviderRequestMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _getProviderResponse = new GetProviderResponse { Name = "Test Provider" };
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockCommitmentsApiClient.Setup(c => c.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getProviderResponse);

            _mapper = new EnterNewTrainingProviderToChangeOfProviderRequestMapper(_mockCommitmentsApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task Provider_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.ProviderId, result.ProviderId);
        }

        [Test, MoqAutoData]
        public async Task ProviderName_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(_getProviderResponse.Name, result.ProviderName);
        }

        [Test, MoqAutoData]
        public async Task EmployerWillAdd_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.EmployerWillAdd, result.EmployerWillAdd);
        }

        [Test, MoqAutoData]
        public async Task NewStartMonth_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.NewStartMonth, result.NewStartMonth);
        }

        [Test, MoqAutoData]
        public async Task NewStartYear_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.NewStartYear, result.NewStartYear);
        }

        [Test, MoqAutoData]
        public async Task NewEndMonth_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.NewEndMonth, result.NewEndMonth);
        }

        [Test, MoqAutoData]
        public async Task NewEndYear_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.NewEndYear, result.NewEndYear);
        }

        [Test, MoqAutoData]
        public async Task NewPrice_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.NewPrice, result.NewPrice);
        }
    }
}
