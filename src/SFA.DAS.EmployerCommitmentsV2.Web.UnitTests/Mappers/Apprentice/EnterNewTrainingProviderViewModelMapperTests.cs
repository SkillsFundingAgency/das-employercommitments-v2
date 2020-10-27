using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class EnterNewTrainingProviderViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        private EnterNewTrainingProviderViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mockCommitmentsApiClient.Setup(m => m.GetAllProviders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockGetAllProvidersResponse());

            _mapper = new EnterNewTrainingProviderViewModelMapper(_mockCommitmentsApiClient.Object);
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
        public async Task WhenRequestingEnterNewTrainingProvider_ThenListOfGetAllProvidersCalled(EnterNewTrainingProviderRequest request)
        {
            var result = await _mapper.Map(request);

            _mockCommitmentsApiClient.Verify(m => m.GetAllProviders(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test, MoqAutoData]
        public async Task WhenRequestingEnterNewTrainingProvider_ThenListOfTrainingProvidersIsMapped(EnterNewTrainingProviderRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(3, result.Providers.Count);
        }
        private GetAllProvidersResponse MockGetAllProvidersResponse()
        {
            return new GetAllProvidersResponse
            {
                Providers = new List<Provider>
                {
                    new Provider { Ukprn = 10000001, Name = "Provider 1" },
                    new Provider { Ukprn = 10000002, Name = "Provider 2" },
                    new Provider { Ukprn = 10000003, Name = "Provider 3" }
                }
            };
        }
    }
}
