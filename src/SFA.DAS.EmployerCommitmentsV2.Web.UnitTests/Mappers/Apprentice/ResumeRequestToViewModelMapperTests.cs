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
    public class ResumeRequestToViewModelMapperTests
    {
        private const string ExpectedFullName = "FirstName LastName";
        private const string ExpectedCourseName = "Test Apprenticeship";
        private const string ExpectedUln = "1234567890";

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
        public async Task ApprenticeshipHashedId_IsMapped(ResumeRequest request)
        {
            var mapper = new ResumeRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(ResumeRequest request)
        {
            var mapper = new ResumeRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeName_IsMapped(ResumeRequest request)
        {
            var mapper = new ResumeRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(ExpectedFullName, result.ApprenticeName);
        }

        [Test, MoqAutoData]
        public async Task CourseName_IsMapped(ResumeRequest request)
        {
            var mapper = new ResumeRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(ExpectedCourseName, result.Course);
        }

        [Test, MoqAutoData]
        public async Task ULN_IsMapped(ResumeRequest request)
        {
            var mapper = new ResumeRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(ExpectedUln, result.ULN);
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
