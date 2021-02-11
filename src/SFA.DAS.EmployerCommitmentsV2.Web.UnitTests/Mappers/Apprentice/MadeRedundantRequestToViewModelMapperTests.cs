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
    public class MadeRedundantRequestToViewModelMapperTests
    {
        private const string ExpectedFullName = "FirstName LastName";

        private Mock<ICommitmentsApiClient> mockCommitmentsApiClient;

        [SetUp]
        public void SetUp()
        {
            mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            mockCommitmentsApiClient
                .Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(GetApprenticeshipResponse());
        }
        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(MadeRedundantRequest request)
        {
            var mapper = new MadeRedundantRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(MadeRedundantRequest request)
        {
            var mapper = new MadeRedundantRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeName_IsMapped(MadeRedundantRequest request)
        {
            var mapper = new MadeRedundantRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(ExpectedFullName, result.ApprenticeName);
        }

        [Test, MoqAutoData]
        public async Task StopMonth_IsMapped(MadeRedundantRequest request)
        {
            var mapper = new MadeRedundantRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.StopMonth, result.StopMonth);
        }

        [Test, MoqAutoData]
        public async Task StopYear_IsMapped(MadeRedundantRequest request)
        {
            var mapper = new MadeRedundantRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.StopYear, result.StopYear);
        }

        private GetApprenticeshipResponse GetApprenticeshipResponse()
        {
            return new GetApprenticeshipResponse
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Uln = "1234567890",
                CourseName = "Test Apprenticeship"
            };
        }
    }
}
