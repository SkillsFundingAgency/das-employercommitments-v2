using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ConfirmHasValidEndDateRequestToViewModelMapperrTests
    {
        private const string ExpectedFullName = "FirstName LastName";
        private const string ExpectedCourseName = "Test Apprenticeship";
        private const string ExpectedUln = "1234567890";
        private DateTime ExpectedStartDateTime = DateTime.Now.AddYears(-2);
        private DateTime ExpectedEndDate = DateTime.Now;

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
        public async Task ApprenticeshipHashedId_IsMapped(ConfirmHasValidEndDateRequest request)
        {
            var mapper = new ConfirmHasValidEndDateRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(ConfirmHasValidEndDateRequest request)
        {
            var mapper = new ConfirmHasValidEndDateRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
        }

        [Test, MoqAutoData]
        public async Task ApprenticeName_IsMapped(ConfirmHasValidEndDateRequest request)
        {
            var mapper = new ConfirmHasValidEndDateRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.That(result.ApprenticeName, Is.EqualTo(ExpectedFullName));
        }

        [Test, MoqAutoData]
        public async Task CourseName_IsMapped(ConfirmHasValidEndDateRequest request)
        {
            var mapper = new ConfirmHasValidEndDateRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.That(result.Course, Is.EqualTo(ExpectedCourseName));
        }

        [Test, MoqAutoData]
        public async Task ULN_IsMapped(ConfirmHasValidEndDateRequest request)
        {
            var mapper = new ConfirmHasValidEndDateRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.That(result.ULN, Is.EqualTo(ExpectedUln));
        }

        [Test, MoqAutoData]
        public async Task EndDate_IsMapped(ConfirmHasValidEndDateRequest request)
        {
            var mapper = new ConfirmHasValidEndDateRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.That(result.EndDate, Is.EqualTo(ExpectedEndDate));
        }

        private GetApprenticeshipResponse GetApprenticeshipResponse()
        {
            return new GetApprenticeshipResponse
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Uln = ExpectedUln,
                CourseName = ExpectedCourseName,
                StartDate = ExpectedStartDateTime,
                EndDate = ExpectedEndDate
            };
        }
    }
}