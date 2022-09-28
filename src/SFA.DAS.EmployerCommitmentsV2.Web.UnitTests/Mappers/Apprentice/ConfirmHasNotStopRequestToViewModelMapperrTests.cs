using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ConfirmHasNotStopRequestToViewModelMapperrTests
    {
        private const string ExpectedFullName = "FirstName LastName";
        private const string ExpectedCourseName = "Test Apprenticeship";
        private const string ExpectedUln = "1234567890";
        private DateTime ExpectedStartDateTime = DateTime.Now.AddYears(-2);

        private Mock<ICommitmentsApiClient> mockCommitmentsApiClient;
        private GetApprenticeshipResponse ApprenticeshipDetails;

        [SetUp]
        public void SetUp()
        {
            mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            ApprenticeshipDetails = GetApprenticeshipResponse();

            mockCommitmentsApiClient
                .Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(() => ApprenticeshipDetails);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(ConfirmHasNotStopRequest request)
        {
            var mapper = new ConfirmHasNotStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(ConfirmHasNotStopRequest request)
        {
            var mapper = new ConfirmHasNotStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeName_IsMapped(ConfirmHasNotStopRequest request)
        {
            var mapper = new ConfirmHasNotStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(ExpectedFullName, result.ApprenticeName);
        }

        [Test, MoqAutoData]
        public async Task CourseName_IsMapped(ConfirmHasNotStopRequest request)
        {
            var mapper = new ConfirmHasNotStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(ExpectedCourseName, result.Course);
        }

        [Test, MoqAutoData]
        public async Task ULN_IsMapped(ConfirmHasNotStopRequest request)
        {
            var mapper = new ConfirmHasNotStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(ExpectedUln, result.ULN);
        }

        private GetApprenticeshipResponse GetApprenticeshipResponse()
        {
            return new GetApprenticeshipResponse
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Uln = ExpectedUln,
                CourseName = ExpectedCourseName,
                StartDate = ExpectedStartDateTime
            };
        }
    }
}